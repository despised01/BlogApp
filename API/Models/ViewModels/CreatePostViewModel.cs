using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace API.Models.ViewModels
{
	public class CreatePostViewModel
	{
		[Required]
		[Display(Name = "Название", Prompt = "Введите название")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Текст", Prompt = "Введите текст")]
		public string PostBody { get; set; }

		[Display(Name = "Описание", Prompt = "Введите описание")]
		public string Comment { get; set; }

		[Required]
		public List<CheckTag> CheckTags { get; set; }

		public CreatePostViewModel() { }
	}
}
