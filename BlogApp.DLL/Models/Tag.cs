using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }

        // Связь с статьями
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
