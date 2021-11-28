using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    class CreatePostViewModel:BaseViewModel
    {
        public CreatePostViewModel()
        {
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT profileName FROM Player WHERE playerID=@playerID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            string profileName = dtbl.Rows[0]["profileName"].ToString();

            App.sqlcon.Close();

            var stream = System.IO.Stream.Null;
            ChooseFile = new Command(async () =>
            {
                var pickResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select Image to Post"
                });

                if (pickResult != null)
                {
                    stream = await pickResult.OpenReadAsync();
                    image = ImageSource.FromStream(() => stream);
                }
            });


            UploadPost = new Command(() =>
            {
                try
                {
                    //string folderPath = @"C:\Users\mjbro\OneDrive\Documents\CUW\CSC491 - Capstone Project\PickupSports\PickupSports.Android\Resources\drawable";
                    //string folderPath = "C:\\Users\\mjbro\\OneDrive\\Documents\\CUW";
                    //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    //string folderpath2 = Directory.GetCurrentDirectory();
                    //string folderPath = @"D:\";
                    //string fileName = Guid.NewGuid() + profileName + ".jpg";
                    //string imagePath = folderPath + fileName;

                
                    //var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
                    //var file1 = files[0];
                    //var file2 = files[1];

                    //DirectoryInfo info = new DirectoryInfo(folderPath);
                    //if (!info.Exists)
                    //{
                    //    info.Create();
                    //}

                    //string path = Path.Combine(folderPath, fileName);
                    //using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                    //{
                    //    stream.CopyTo(outputFileStream);
                    //}
                }
                catch(Exception e)
                {
                    string error = e.ToString();
                }

            });

            Cancel = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        ImageSource imageVal;
        public ImageSource image { get => imageVal; set => SetProperty(ref imageVal, value); }

        public Command ChooseFile { get; }

        public Command UploadPost { get; }

        public Command Cancel { get; }
    }
}
