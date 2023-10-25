using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DLL.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string BodyText { get; set; }

        // Автор
        public Guid? Author_Id { get; set; }
        public User Author { get; set; }

        // Статья
        public Guid? Post_Id { get; set; }
        public Post Post { get; set; }
    }
}
