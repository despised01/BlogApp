using API.Views.Pages;
using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Views
{
    public class PostUpdatePageModel : PageModel
    {
        private IPostRepository _posts;
        private ITagRepository _tags;
        private ICommentRepository _comments;
        private IMapper _mapper;
        public List<CheckTag> CheckTags { get; set; }
        public object? Id { get; private set; }

        public Post post { get; set; }

        [Required]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Name { get; set; }

        [Required]
        public string PostBody { get; set; }

        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Comment { get; set; }

        public PostUpdatePageModel(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            _posts = postRepository;
            _tags = tagRepository;
            _comments = commentRepository;
            _mapper = mapper;
        }

        public async void OnGet()
        {
            Id = RouteData.Values["id"];
            Guid guid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromString((string)RouteData.Values["id"]);
            post = await _posts.Get(guid);

            CheckTags = new List<CheckTag>();
            var allTags = _tags.GetAll().Result;

            foreach (var existTag  in allTags)
            {
                var tmp = new CheckTag();
                tmp.RememberMe = false;
                tmp.tagName = existTag.TagName;
                CheckTags.Add(tmp);
            }
        }
    }
}
