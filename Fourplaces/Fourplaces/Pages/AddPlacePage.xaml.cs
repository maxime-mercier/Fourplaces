using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPlacePage
    {
		public AddPlacePage ()
		{
			InitializeComponent ();
            BindingContext = new AddPlaceViewModel(Navigation);
		}
	}
}