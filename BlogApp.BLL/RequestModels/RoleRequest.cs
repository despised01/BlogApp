using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.RequestModels
{
	public class RoleRequest
	{
		public RoleRequest()
		{
		}

		public RoleRequest(Guid Id, string Name)
		{
			this.Id = Id;
			this.Name = Name;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
