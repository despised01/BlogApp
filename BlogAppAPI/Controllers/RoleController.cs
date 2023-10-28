using API.Controllers;
using API.Models.ViewModels;
using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;
using BlogApp.DLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private IRoleRepository roles;
        private IMapper mapper;
        private IAccountController accountController;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public RoleController(IRoleRepository roleRepository, IMapper mapper, IAccountController accountController)
        {
            roles = roleRepository;
            this.mapper = mapper;
            this.accountController = accountController;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await roles.Get(id);
            if (role != null)
                return StatusCode(200, role);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetAll")]
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
            _logger.Info("RoleController : Create");
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
            _logger.Info("RoleController : Update");
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
            _logger.Info("RoleController : Delete");
            var role = await roles.Get(id);
            if (role != null)
            {
                await roles.Delete(role);
                return StatusCode(200);
            }
            return NotFound();
        }
        [Route("CreateRole")]
        public IActionResult CreateRole()
        {
            return RedirectToPage("/CreateRolePage");
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(CreateRoleViewModel model)
        {
            _logger.Info("RoleController : AddRole");
            var allroles = await roles.GetAll();
            var temp = allroles.Where(x => x.Name == model.Name).FirstOrDefault();
            if (temp != null) return StatusCode(400);
            RoleRequest request = new RoleRequest();
            request.Id = Guid.NewGuid();
            request.Name = model.Name;

            var result = Create(request);

            return RedirectToPage("/Index");
        }

        //GetRoleToUpdate
        [Route("GetRoleToUpdate/{id?}")]
        public IActionResult GetRoleToUpdate(CreateRoleViewModel model, [FromRoute] Guid ID)
        {
            return RedirectToPage("/RoleUpdatePage", new { id = ID.ToString() });
        }

        [Route("RoleUpdateById/{id?}")]
        public async Task<IActionResult> RoleUpdateById(CreateRoleViewModel model, [FromRoute] Guid ID)
        {
            _logger.Info("RoleController : RoleUpdateById");
            try
            {
                var tmpRole = await GetById(ID);
                var role = (Role)(tmpRole as ObjectResult).Value;
                role.Name = model.Name;
                role.Description = model.Comment;

                await roles.Update(role);
            }
            catch { }


            return RedirectToPage("/RolesPage");
        }
    }
}
