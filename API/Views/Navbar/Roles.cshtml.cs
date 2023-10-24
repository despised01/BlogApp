using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Views.Navbar
{
    public class RolesModel : PageModel
    {
        private IUserRepository users;
        private IRoleRepository roles;
        private IMapper mapper;

        public List<Role> Roles { get; set; }

        public RolesModel(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            users = userRepository;
            roles = roleRepository;
            this.mapper = mapper;
        }


        public async void OnGet()
        {
            Roles = new List<Role>();
            var allRoles = roles.GetAll().Result;
            Roles.AddRange(allRoles);
        }
    }
}
