using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogApp.DLL.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        // Статьи пользователя
        public List<Post>? Posts { get; set; }

        // Комментарии пользователя
        public List<Comment>? Comments { get; set; }

        // Привязываю роли многие ко многим
        public List<Role>? Roles { get; set; }

    }
}
