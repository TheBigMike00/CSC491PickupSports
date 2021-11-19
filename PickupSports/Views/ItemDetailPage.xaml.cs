using System.ComponentModel;
using Xamarin.Forms;
using PickupSports.ViewModels;

namespace PickupSports.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}