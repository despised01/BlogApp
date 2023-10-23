using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Views
{
    public class RoleUpdatePageModel : PageModel
    {
        private IRoleRepository roles;
        private IMapper mapper;

        public Role role = null;

        [Required]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Name { get; set; }

        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Comment { get; set; }

        public object? Id { get; private set; }

        public RoleUpdatePageModel(IRoleRepository roles, IMapper mapper)
        {
            this.roles = roles;
            this.mapper = mapper;
        }

        public async void OnGet()
        {
            Id = RouteData.Values["id"];
            Guid guid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromString((string)RouteData.Values["id"]);
            role = await roles.Get(guid);
        }
    }
}
