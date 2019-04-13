using Fourplaces.Model;
using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageDetail
    {

        public PageDetail(int selectedPlaceId)
        {
            InitializeComponent();
            BindingContext = new PageDetailViewModel(selectedPlaceId, MyMap, Navigation);
        }
	}
}