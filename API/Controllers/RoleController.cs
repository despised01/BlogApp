using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private IRoleRepository roles;
		private IMapper mapper;

		public RoleController(IRoleRepository roleRepository, IMapper mapper)
		{
			roles = roleRepository;
			this.mapper = mapper;
		}

		[HttpGet]
		[Route("GetById")]
		public async Task<IActionResult> GetAll()
		{
			var allTags = await roles.GetAll();
			if (allTags != null)
				return StatusCode(200, allTags);
			else
				return NoContent();
		}

		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> Create(RoleRequest request)
		{
			if (request.Id.ToString() == "" || await roles.Get(request.Id) == null)
			{
				var role = mapper.Map<RoleRequest, Role>(request);
				await roles.Create(role);
				return StatusCode(200);
			}
			else
				return StatusCode(400, "Уже существует");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(RoleRequest request)
		{
			if (await roles.Get(request.Id) != null)
			{
				var role = mapper.Map<RoleRequest, Role>(request);
				await roles.Update(role);
				return StatusCode(200);
			}
			else
				return NotFound();
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var role = await roles.Get(id);
			if (role != null)
			{
				await roles.Delete(role);
				return StatusCode(200);
			}
			else
				return NotFound();
		}
	}
}
