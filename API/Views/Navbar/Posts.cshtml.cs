using AutoMapper;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Views.Navbar
{
    public class PostsModel : PageModel
    {
        private IPostRepository _posts;
        private IMapper mapper;
        public List<Post> posts { get; set; }

        public PostsModel(IPostRepository posts, IMapper mapper)
        {
            _posts = posts;
            this.mapper = mapper;
        }

        public async void OnGet()
        {
            posts = new List<Post>();

            var allPosts = _posts.GetAll().Result;
            posts.AddRange(allPosts);
        }
    }
}
