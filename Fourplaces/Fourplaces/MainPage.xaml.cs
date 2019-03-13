using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;

namespace Fourplaces
{
    public partial class MainPage : BaseContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new PlaceListViewModel(Navigation);
        }
    }
}
