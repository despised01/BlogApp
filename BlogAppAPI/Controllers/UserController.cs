using API.Models.ViewModels;
using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepository users;
        private IRoleRepository roles;
        private IMapper mapper;
        private readonly IHttpContextAccessor _http;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UserController(IUserRepository userRepository, IRoleRepository role, IMapper mapper, IHttpContextAccessor http)
        {
            users = userRepository;
            roles = role;
            _http = http;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var allUsers = await users.GetAll();
            if (allUsers != null)
            {
                return StatusCode(200, allUsers);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await users.Get(id);
            if (user != null)
                return StatusCode(200, user);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetByLogin")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            var user = await users.GetByLogin(login);
            if (user != null)
                return StatusCode(200, user);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(UserRequest request)
        {
            Guid guid = Guid.NewGuid();
            if (await users.Get(guid) == null)
            {
                var newUser = mapper.Map<UserRequest, User>(request);
                newUser.Id = guid;
                await users.Create(newUser);
                return StatusCode(200);
            }
            else
                return StatusCode(400, "Уже существует");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UserRequest request)
        {
            if (await users.Get(request.Id) != null)
            {
                var newUser = mapper.Map<UserRequest, User>(request);
                await users.Update(newUser);
                return StatusCode(200);
            }
            else
                return NotFound();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.Info("UserController : UserUpdate");
            var user = await users.Get(id);
            if (user != null)
            {
                await users.Delete(user);
                return StatusCode(200);
            }
            return NotFound();
        }

        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> UpdateRole(RoleRequest reqest, Guid userId)
        {
            _logger.Info("UserController : UserUpdate");
            var role = await roles.GetByName(reqest.Name);
            var user = await users.Get(userId);
            if (user != null && role != null)
            {
                user.Roles.Add(role);
                await users.Update(user);
                return StatusCode(200);
            }
            else
                return NotFound();
        }

        [Route("CreateUser")]
        public IActionResult CreateUser()
        {
            return RedirectToPage("/RegistrationPage");
        }

        [HttpPost]
        [Route("UserUpdate")]
        public async Task<IActionResult> UserUpdate(UserUpdateViewModel request)
        {
            _logger.Info("UserController : UserUpdate");
            try
            {
                Console.WriteLine("UserUpdate");
                List<Role> requastRoles = new List<Role>();

                var allRoles = await roles.GetAll();

                foreach (var c in request.CheckRoles)
                {
                    var tmp = allRoles.FirstOrDefault(x => x.Name == c.roleName & c.RememberMe);
                    if (tmp != null)
                        requastRoles.Add(tmp);
                }

                var user = await users.GetByLogin(request.Login);

                user.Roles = requastRoles;
                user.Email = request.Email;
                user.Password = request.Password;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                users.Update(user);
            }
            catch { Console.WriteLine("UserUpdate : Error"); }

            return RedirectToPage("/Index");
        }

        [Route("GetUserToUpdate/{id?}")]
        public IActionResult GetUserToUpdate(RegistrationViewModel model, [FromRoute] Guid ID)
        {
            return RedirectToPage("/UserUpdatePage", new { id = ID.ToString() });
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        public string GetCurrentUser()
        {
            return _http.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }

        [HttpPost]
        [Route("UserUpdateById/{id?}")]
        public async Task<IActionResult> UserUpdateById(UserUpdateViewModel request, [FromRoute] Guid ID)
        {
            _logger.Info("UserController : UserUpdateById");
            try
            {
                List<Role> requastRoles = new List<Role>();

                var allRoles = await roles.GetAll();

                foreach (var c in request.CheckRoles)
                {
                    var tmp = allRoles.FirstOrDefault(x => x.Name == c.roleName & c.RememberMe);
                    if (tmp != null)
                        requastRoles.Add(tmp);
                }

                var user = await users.Get(ID);

                user.Login = request.Login;
                user.Roles = requastRoles;
                user.Email = request.Email;
                user.Password = request.Password;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                users.Update(user);
            }
            catch { Console.WriteLine("UserUpdate : Error"); }

            return RedirectToPage("/Index");
        }
    }
}
