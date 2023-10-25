using BlogApp.BLL.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.Validators
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            RuleFor(x => x.BodyText).NotEmpty();
        }
    }
}
