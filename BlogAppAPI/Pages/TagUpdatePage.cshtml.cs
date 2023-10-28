using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Views.Pages
{
    public class TagUpdatePageModel : PageModel
    {
        private ITagRepository tags;
        private IMapper mapper;
        public Tag tag { get; set; }
        public object? Id { get; private set; }

        [Required]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Name { get; set; }

        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string? Comment { get; set; }

        public TagUpdatePageModel(ITagRepository tags, IMapper mapper)
        {
            this.tags = tags;
            this.mapper = mapper;
        }
        public async Task OnGetAsync()
        {
            Id = RouteData.Values["id"];
            Guid guid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromString((string)RouteData.Values["id"]);
            tag = await tags.Get(guid);
        }
    }
}
