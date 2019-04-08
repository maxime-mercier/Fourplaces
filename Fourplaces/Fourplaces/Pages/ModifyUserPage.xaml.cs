using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fourplaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyUserPage : ContentPage
	{
		public ModifyUserPage (bool isModifyPasswordPage)
		{
			InitializeComponent ();
            BindingContext = new ModifyUserViewModel(isModifyPasswordPage, Navigation);
        }
	}
}