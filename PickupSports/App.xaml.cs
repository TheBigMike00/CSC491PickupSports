using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PickupSports.Services;
using PickupSports.Views;
using System.Data.SqlClient;

namespace PickupSports
{
    public partial class App : Application
    {
        public static Guid playerID;
        public static Guid tempPlayerID;
        //public static Guid playerID = Guid.Parse("AAAAAAA3-BBBB-CCCC-DDDD-EEEEEEEEEEEE");
        public static SqlConnection sqlcon = new SqlConnection("Data Source=LAPTOP-NS5R2J8I;Initial Catalog=PickupSportsDB;User Id=mbrocker;Password=mbrocker;");
        

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();
            MainPage = new LoginPage();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
