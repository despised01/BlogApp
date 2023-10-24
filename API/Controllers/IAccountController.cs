using API.Models.ViewModels;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public interface IAccountController
    {
        public Task<User> Authenticate(UserRequest request, string login, string password);
        public Task<IActionResult> Login(LoginViewModel model);
        public Task<IActionResult> Registration(RegistrationViewModel model);
        public Task<User> GetCurrentUser();
    }
}
