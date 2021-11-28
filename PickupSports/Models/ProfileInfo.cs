using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using PickupSports.ViewModels;

namespace PickupSports.Models
{
    public class ProfileInfo:BaseViewModel
    {
        int ageVal;
        public int age { get => ageVal; set => SetProperty(ref ageVal, value); }

        int heightVal;
        public int height { get => heightVal; set => SetProperty(ref heightVal, value); }

        string displayableHeightVal;
        public string displayableHeight { get => displayableHeightVal; set => SetProperty(ref displayableHeightVal, value); }

        int weightVal;
        public int weight { get => weightVal; set => SetProperty(ref weightVal, value); }

        int verticalVal;
        public int vertical { get => verticalVal; set => SetProperty(ref verticalVal, value); }
    }
}
