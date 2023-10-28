using AutoMapper;
using AutoMapper.Execution;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace API.Views.Pages
{
    public class PostPageModel : PageModel
    {
        private IPostRepository _posts;
        private ITagRepository _tags;
        private ICommentRepository _comments;
        private IMapper _mapper;
        public object? Id { get; private set; }

        public Post post { get; set; }

        public string BodyText { get; set; }
        public Guid postId { get; set; }

        public PostPageModel(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IMapper mapper)
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
        }
    }
}
