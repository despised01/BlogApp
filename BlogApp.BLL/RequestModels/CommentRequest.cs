using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.RequestModels
{
    public class CommentRequest
    {
        public Guid Id { get; set; }
        public string BodyText { get; set; }
        public Guid postId { get; set; }
    }
}
