using Fourplaces.Model;
using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageDetail : BaseContentPage
	{

        public PageDetail(PlaceItemSummary SelectedPlace)
        {
            InitializeComponent();
            BindingContext = new PageDetailViewModel(SelectedPlace, MyMap, Navigation);
        }
	}
}