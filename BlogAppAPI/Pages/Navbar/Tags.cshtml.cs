using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Views.Navbar
{
    public class TagsModel : PageModel
    {

        private ITagRepository _tags;
        private IRoleRepository roles;
        private IMapper mapper;

        public List<Tag> tags { get; set; }

        public TagsModel(ITagRepository tagRepository, IMapper mapper)
        {
            _tags = tagRepository;
            this.mapper = mapper;
        }

        public async void OnGet()
        {
            tags = new List<Tag>();
            var allTags = _tags.GetAll().Result;
            tags.AddRange(allTags);
        }
    }
}
