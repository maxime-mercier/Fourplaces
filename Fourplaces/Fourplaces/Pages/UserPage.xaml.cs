using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserPage : BaseContentPage
	{

		public UserPage ()
		{
			InitializeComponent ();
            BindingContext = new UserPageViewModel(Navigation);
        }
	}
}