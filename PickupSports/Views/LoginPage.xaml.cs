using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickupSports.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickupSports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : TabbedPage
    {
        public LoginPage()
        {
            InitializeComponent();
            //this.BindingContext = new LoginViewModel();
            //this.BackgroundImageSource = "login_background.jpg";
        }
    }
}