using API.Models.ViewModels;
using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.Controllers;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using System.Security.Claims;
using NLog;

namespace API.Controllers
{
    public class AccountController : Controller, IAccountController
    {
        private IUserRepository users;
        private IRoleRepository roles;
        private IMapper mapper;
        private UserController userController;
        private readonly IHttpContextAccessor _http;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AccountController(IUserRepository userRepository, IRoleRepository role, IMapper mapper, UserController userController, IHttpContextAccessor http)
        {
            users = userRepository;
            roles = role;
            this.mapper = mapper;
            this.userController = userController;
            _http = http;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<User> Authenticate(UserRequest request, string login, string password)
        {
            _logger.Info("AccountController : Authenticate");
            var user = users.GetByLogin(login).Result;
            if (user.Login != login)
                throw new AuthenticationException("Неверный логин");

            if (user.Password != password)
                throw new AuthenticationException("Неверный пароль");

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login), //request.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, request.Role.Name)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "AddCookies",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return user;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.Info("AccountController : Login");
            var user = await users.GetByLogin(model.Login);

            if (user == null)
                return RedirectToPage("/LoginPage");

            UserRequest request = new UserRequest();

            request.Role = new RoleRequest(Guid.Empty, "User");
            try
            {
                if (user.Roles.Where(x => x.Name == "Admin") != null)
                {
                    request.Role.Name = "Admin";
                }
                else if (user.Roles.Where(x => x.Name == "Moderator") != null)
                {
                    request.Role.Name = "Moderator";
                }
            }
            catch (Exception ex) { }

            var result = Authenticate(request, model.Login, model.Password);

            return RedirectToPage("/Index");
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            _logger.Info("AccountController : Registration");
            var user = await users.GetByLogin(model.Login);

            if (user != null)
                return RedirectToPage("/RegistrationPage");

            UserRequest request = new UserRequest();
            request.Role = new RoleRequest(Guid.Empty, "User");
            request.FirstName = model.FirstName;
            request.LastName = model.LastName;
            request.Email = model.Email;
            request.Password = model.Password;
            request.Login = model.Login;

            Guid guid = Guid.NewGuid();
            if (await users.Get(guid) == null)
            {
                var newUser = mapper.Map<UserRequest, User>(request);
                newUser.Id = guid;
                await users.Create(newUser);
            }


            return RedirectToPage("/Index");
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<User> GetCurrentUser()
        {
            _logger.Info("AccountController : GetCurrentUser");
            var userLogin = _http.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var user = ((await userController.GetByLogin(userLogin)) as ObjectResult);
            return (User)user.Value;
        }
    }
}
