using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;

namespace Fourplaces
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
