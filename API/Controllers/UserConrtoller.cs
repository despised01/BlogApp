using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace BlogApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserConrtoller : ControllerBase
	{
		private IUserRepository users;
		private IRoleRepository roles;
		private IMapper mapper;

		public UserConrtoller(IUserRepository userRepository, IRoleRepository role, IMapper mapper)
		{
			users = userRepository;
			roles = role;
			this.mapper = mapper;
		}

		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var allUsers = await users.GetAll();
			if (allUsers != null)
				return StatusCode(200, allUsers);
			else
				return NoContent();
		}

		[HttpGet]
		[Route("GetById")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var user = await users.Get(id);
			if (user != null)
				return StatusCode(200, user);
			else
				return NotFound();
		}

		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> Create(UserRequest request)
		{
			Guid guid = Guid.NewGuid();
			if (await users.Get(guid) == null)
			{
				var newUser = mapper.Map<UserRequest, User>(request);
				newUser.Id = guid;
				await users.Create(newUser);
				return StatusCode(200);
			}
			else
				return StatusCode(400, "Уже существует");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(UserRequest request)
		{
			if (await users.Get(request.Id) != null)
			{
				var newUser = mapper.Map<UserRequest, User>(request);
				await users.Update(newUser);
				return StatusCode(200);
			}
			else
				return NotFound();
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("Delete")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var user = await users.Get(id);
			if (user != null)
			{
				await users.Delete(user);
				return StatusCode(200);
			}
			else
				return NotFound();
		}

		[HttpPut]
		[Route("UpdateRole")]
		public async Task<IActionResult> UpdateRole(RoleRequest request, Guid userId)
		{
			var role = await roles.GetByName(request.Name);
			var user = await users.Get(userId);
			if (user != null && role != null)
			{
				user.Roles.Add(role);
				await users.Update(user);
				return StatusCode(200);
			}
			else
				return NotFound();
		}

		[HttpPost]
		[Route("Authenticate")]
		public async Task<User> Authenticate(UserRequest request, string login, string password)
		{
			var user = users.GetByLogin(login).Result;
			if (user.Login != login)
				throw new AuthenticationException("Неверный логин!");

			if (user.Password != password)
				throw new AuthenticationException("Неверный пароль!");

			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, request.Role.Name)
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(
				claims,
				"AddCookies",
				ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

			return user;
		}
	}
}
