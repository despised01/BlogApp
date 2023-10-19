using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
	public class RegistrationViewModel
	{
		[Required(ErrorMessage = "Поле Nickname обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Nickname", Prompt = "Nickname")]
		public string Login { get; set; }

		[Required(ErrorMessage = "Поле Email обязательно для заполнения")]
		[EmailAddress]
		[Display(Name = "Email", Prompt = "Email@mail.ru")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль", Prompt = "**********")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Обязательно подтвердите пароль")]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль", Prompt = "**********")]
		public string PasswordConfirm { get; set; }

		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Имя", Prompt = "Имя")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Фамилия", Prompt = "Фамилия")]
		public string LastName { get; set; }
	}
}
