using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickupSports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePostPage : ContentPage
    {
        public CreatePostPage()
        {
            InitializeComponent();
        }

        //async void chooseFileClicked(System.Object sender, System.EventArgs e)
        //{
        //    var pickResult = await FilePicker.PickAsync(new PickOptions
        //    {
        //        FileTypes = FilePickerFileType.Images,
        //        PickerTitle = "Select Image to Post"
        //    });

        //    if(pickResult != null)
        //    {
        //        var stream = await pickResult.OpenReadAsync();
        //        selectedImage.Source = ImageSource.FromStream(() => stream);
        //    }
        //}

    }
}