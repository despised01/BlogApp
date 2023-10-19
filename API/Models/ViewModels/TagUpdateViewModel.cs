using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
	public class TagUpdateViewModel
	{
		[Required]
		[Display(Name = "Название", Prompt = "Введите название")]
		public string Name { get; set; }

		[Display(Name = "Описание", Prompt = "Введите описание")]
		public string? Comment { get; set; }

		public TagUpdateViewModel() { }
	}
}
