using API.Controllers;
using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private ICommentRepository comments;
        private IAccountController accountController;
        private UserController userController;
        private IPostRepository posts;
        private IMapper mapper;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public CommentController(ICommentRepository commentRepository, IAccountController _accountController, UserController userController,
            IPostRepository postRepository, IMapper mapper)
        {
            comments = commentRepository;
            accountController = _accountController;
            this.userController = userController;
            this.posts = postRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var allComments = await comments.GetAll();
            if (allComments != null)
            {
                return StatusCode(200, allComments);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await comments.Get(id);
            if (comment != null)
                return StatusCode(200, comment);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CommentRequest request, Guid postId)
        {
            _logger.Info("CommentController : Create");
            if (request.Id.ToString() == "" || await comments.Get(request.Id) == null)
            {
                var newComment = mapper.Map<CommentRequest, Comment>(request);
                var user = accountController.GetCurrentUser();
                var post = posts.Get(postId);


                newComment.Id = Guid.NewGuid();
                newComment.BodyText = request.BodyText;
                newComment.Author = user.Result;
                newComment.Post = await post;
                newComment.Author_Id = user.Result.Id;
                newComment.Post_Id = postId;
                await comments.Create(newComment);
                return StatusCode(200);
            }
            else
                return StatusCode(400, "Уже существует");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(CommentRequest request)
        {
            _logger.Info("CommentController : Update");
            if (await comments.Get(request.Id) != null)
            {
                var newComment = mapper.Map<CommentRequest, Comment>(request);
                await comments.Update(newComment);
                return StatusCode(200);
            }
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.Info("CommentController : Delete");
            var user = await comments.Get(id);
            if (user != null)
            {
                await comments.Delete(user);
                return StatusCode(200);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("CreateComment/{id?}")]
        public async Task<IActionResult> CreateComment(CommentRequest model, [FromRoute] Guid ID)
        {
            _logger.Info("CommentController : CreateComment");
            model.Id = Guid.NewGuid();

            await Create(model, ID);

            return RedirectToPage("/PostPage", new { id = ID.ToString() });
        }
    }
}
