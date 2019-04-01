using Fourplaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage
	{
		public SignInPage ()
		{
			InitializeComponent ();
            BindingContext = new SignInViewModel(Navigation);
        }
	}
}