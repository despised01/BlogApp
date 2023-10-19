using BlogApp.DLL.Models;

namespace API.Models.ViewModels
{
	public class RoleUpdateViewModel
	{
		public bool RememberMe { get; set; } = false;
		public string roleName { get; set; }
		public string? roleDescription { get; set; }
		public RoleUpdateViewModel(bool rememberMe, Role role)
		{
			RememberMe = rememberMe;
			this.roleName = role.Name;
		}

		public RoleUpdateViewModel() { }
	}
}
