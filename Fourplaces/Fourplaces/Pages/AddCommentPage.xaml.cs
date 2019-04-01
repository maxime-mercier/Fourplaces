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
	public partial class AddCommentPage : ContentPage
	{
		public AddCommentPage (int placeId)
		{
			InitializeComponent ();
            BindingContext = new AddCommentVIewModel(placeId, Navigation);
        }
	}
}