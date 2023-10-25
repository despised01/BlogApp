using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.RequestModels
{
    public class TagRequest
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}
