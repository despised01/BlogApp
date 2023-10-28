using API.Controllers;
using API.Models.ViewModels;
using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private IPostRepository posts;
        private ITagRepository tags;
        private IUserRepository _users;
        private IAccountController accountController;
        private IMapper mapper;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PostController(IPostRepository postRepository, ITagRepository tagRepository, IUserRepository userRepository,
            IAccountController accountController, IMapper mapper)
        {
            posts = postRepository;
            tags = tagRepository;
            _users = userRepository;
            this.accountController = accountController;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var allPost = await posts.GetAll();
            if (allPost != null)
            {
                return StatusCode(200, allPost);
            }
            else
                return NoContent();
        }
        [HttpGet]
        [Route("GetAllByAuthor")]
        public async Task<IActionResult> GetAllByAuthorId(Guid authorGuid)
        {
            var allPost = await posts.GetAllByAuthorId(authorGuid);

            if (allPost != null)
            {
                return StatusCode(200, allPost);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await posts.Get(id);
            if (post != null)
                return StatusCode(200, post);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(PostRequest request)
        {
            if (request.Id.ToString() == "" || await posts.Get(request.Id) == null)
            {
                var newPost = mapper.Map<PostRequest, Post>(request);
                await posts.Create(newPost);
                return StatusCode(200);
            }
            else
                return StatusCode(400, "Уже существует");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(PostRequest request)
        {
            if (await posts.Get(request.Id) != null)
            {
                var newPost = mapper.Map<PostRequest, Post>(request);
                await posts.Update(newPost);
                return StatusCode(200);
            }
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var newPost = await posts.Get(id);
            if (newPost != null)
            {
                await posts.Delete(newPost);
                return StatusCode(200);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost(CreatePostViewModel model)
        {
            _logger.Info("PostController : AddPost");
            try
            {
                User user = null;
                IEnumerable<Claim> claims;
                string Login = string.Empty;

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    claims = identity.Claims;

                    if (claims != null)
                    {
                        foreach (Claim claim in claims)
                        {
                            if (claim.Type == ClaimTypes.Name)
                            {
                                Login = claim.Value;
                            }
                        }
                    }
                    if (Login != string.Empty)
                    {
                        user = await _users.GetByLogin(Login);
                    }
                }

                Console.WriteLine("AddPost");
                List<Tag> requastTags = new List<Tag>();

                var allTags = await tags.GetAll();

                foreach (var c in model.CheckTags)
                {
                    var tmp = allTags.FirstOrDefault(x => x.TagName == c.tagName & c.RememberMe);
                    if (tmp != null)
                        requastTags.Add(tmp);
                }

                Post post = new Post();
                post.Tags = requastTags;
                post.BodyText = model.PostBody;
                post.CreateTime = DateTime.Now;
                post.Title = model.Name;
                post.Author = user;

                CreatePost(post);
            }
            catch { }

            return RedirectToPage("/Index");
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(Post post)
        {
            _logger.Info("PostController : CreatePost");
            if (post.Id.ToString() == "" || await posts.Get(post.Id) == null)
            {
                await posts.Create(post);
                return StatusCode(200);
            }
            else
                return StatusCode(400, "Уже существует");
        }


        [Route("CreatePostRedirect")]
        public IActionResult CreatePostRedirect()
        {
            return RedirectToPage("/CreatePostPage");
        }

        [Route("GetPostToUpdate/{id?}")]
        public IActionResult GetPostToUpdate(PostUpdateViewModel model, [FromRoute] Guid ID)
        {
            return RedirectToPage("/PostUpdatePage", new { id = ID.ToString() });
        }

        [Route("PostToDelete/{id?}")]
        public IActionResult PostToDelete(PostUpdateViewModel model, [FromRoute] Guid ID)
        {
            _logger.Info("PostController : PostToDelete");
            var result = Delete(ID);

            return RedirectToPage("/Navbar/Posts");
        }

        [Route("PostOpen/{id?}")]
        public IActionResult PostOpen([FromRoute] Guid ID)
        {
            return RedirectToPage("/PostPage", new { id = ID.ToString() });
        }


        [Route("PostUpdateById/{id?}")]
        public async Task<IActionResult> PostUpdateById(CreatePostViewModel model, [FromRoute] Guid ID)
        {
            _logger.Info("PostController : PostUpdateById");
            try
            {
                IEnumerable<Claim> claims;
                string Login = string.Empty;

                var user = await accountController.GetCurrentUser();

                Console.WriteLine("AddPost");
                List<Tag> requestTags = new List<Tag>();

                var allTags = await tags.GetAll();

                foreach (var c in model.CheckTags)
                {
                    var tmp = allTags.FirstOrDefault(x => x.TagName == c.tagName & c.RememberMe);
                    if (tmp != null)
                        requestTags.Add(tmp);
                }

                var tmpPost = await GetById(ID);
                var post = (Post)(tmpPost as ObjectResult).Value;
                post.Tags = requestTags;
                post.BodyText = model.PostBody;
                post.CreateTime = DateTime.Now;
                post.Title = model.Name;
                post.Author = user;

                await posts.Update(post);
            }
            catch { }


            return RedirectToPage("/PostPage", new { id = ID.ToString() });
        }

    }
}
