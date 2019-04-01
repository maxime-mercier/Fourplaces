using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;

namespace Fourplaces.Pages
{
    public partial class PlaceListPage : BaseContentPage
    {
        public PlaceListPage()
        {
            InitializeComponent();
            BindingContext = new PlaceListViewModel(Navigation);
        }
    }
}
