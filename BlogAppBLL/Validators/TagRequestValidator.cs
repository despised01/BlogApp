using BlogApp.BLL.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.Validators
{
	public class TagRequestValidator : AbstractValidator<TagRequest>
	{
		public TagRequestValidator()
		{
			RuleFor(x => x.TagName).NotEmpty().MaximumLength(50);
		}
	}
}
