using System;
using System.Collections.Generic;
using PickupSports.ViewModels;
using PickupSports.Views;
using Xamarin.Forms;

namespace PickupSports
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
