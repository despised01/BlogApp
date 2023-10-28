namespace API.Models.ViewModels
{
	public class MainViewModel
	{
		public LoginViewModel LoginView { get; set; }


		public MainViewModel()
		{
			LoginView = new LoginViewModel();
		}
	}
}
