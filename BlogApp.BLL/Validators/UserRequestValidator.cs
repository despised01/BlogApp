using BlogApp.BLL.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.BLL.Validators
{
	public class UserRequestValidator : AbstractValidator<UserRequest>
	{
		public UserRequestValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
			RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
			RuleFor(x => x.Login).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Birthday).NotEmpty();
		}
	}
}
