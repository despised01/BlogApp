using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TagController : ControllerBase
	{
		private ITagRepository tags;
		private IMapper mapper;

		public TagController(ITagRepository tagRepository, IMapper mapper)
		{
			tags = tagRepository;
			this.mapper = mapper;
		}

		[HttpGet]
		[Route("GetById")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var tag = await tags.Get(id);
			if (tag != null)
				return StatusCode(200, tag);
			else
				return NotFound();
		}

		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var allTags = await tags.GetAll();
			if (allTags != null)
				return StatusCode(200, allTags);
			else
				return NoContent();
		}

		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> Create(TagRequest request)
		{
			if (request.Id.ToString() == "" || await tags.Get(request.Id) == null)
			{
				var newTag = mapper.Map<TagRequest, Tag>(request);
				await tags.Create(newTag);
				return StatusCode(200);
			}
			else
				return StatusCode(400, "Уже существует");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(TagRequest request)
		{
			if (await tags.Get(request.Id) != null)
			{
				var newTag = mapper.Map<TagRequest, Tag>(request);
				await tags.Update(newTag);
				return StatusCode(200);
			}
			else
				return NotFound();
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var tag = await tags.Get(id);
			if (tag != null)
			{
				await tags.Delete(tag);
				return StatusCode(200);
			}
			else
				return NotFound();
		}
	}
}
