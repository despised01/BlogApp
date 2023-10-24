using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace API.Views.Pages
{
    public class UserUpdatePageModel : PageModel
    {
        private IUserRepository users;
        private IRoleRepository roles;
        private IMapper mapper;

        public List<CheckRole> CheckRoles { get; set; }
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле Nickname обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Nickname", Prompt = "Nickname")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email@mail.ru")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "**********")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "**********")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        public string LastName { get; set; }

        public List<RoleRequest> Roles { get; set; }

        public UserUpdatePageModel(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            users = userRepository;
            roles = roleRepository;
            this.mapper = mapper;
        }

        public void OnGet()
        {
            CheckRoles = new List<CheckRole>();
            var allRoles = roles.GetAll().Result;

            foreach (var existTag in allRoles)
            {
                var tmp = new CheckRole();
                tmp.RememberMe = false;
                tmp.roleName = existTag.Name;
                CheckRoles.Add(tmp);
            }
        }

        public void OnPost()
        {

        }
    }

    public class CheckRole
    {
        public bool RememberMe { get; set; } = false;
        public string roleName { get; set; }
        public CheckRole(bool rememberMe, Role role)
        {
            RememberMe = rememberMe;
            roleName = role.Name;
        }

        public CheckRole() { }
    }
}
