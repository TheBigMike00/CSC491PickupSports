using PickupSports.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    class UpdateProfileViewModel:BaseViewModel
    {
        public UpdateProfileViewModel(ProfileViewModel pvm)
        {
            profile = pvm.profile;
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT age, height, weight, vertical, profilePic FROM Player WHERE playerID=@playerID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);

            try
            {
                profile.age = Convert.ToInt32(dtbl.Rows[0]["age"].ToString());
                profile.height = Convert.ToInt32(dtbl.Rows[0]["height"].ToString());
                profile.displayableHeight = pvm.convertInches(profile.height);
                profile.weight = Convert.ToInt32(dtbl.Rows[0]["weight"].ToString());
                profile.vertical = Convert.ToInt32(dtbl.Rows[0]["vertical"].ToString());
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

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
                    profilePic = ImageSource.FromStream(() => stream);
                }
            });


            SaveChanges = new Command(async () =>
            {
                SaveChangesDB();
                await App.Current.MainPage.Navigation.PopAsync();

            });


            Cancel = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        public UpdateProfileViewModel()
        {

        }

        public ProfileInfo profile { get; set; }

        ImageSource profilePic { get; set; }

        public Command ChooseFile { get; }

        public Command SaveChanges { get; }

        public Command Cancel { get; }

        private void SaveChangesDB()
        {
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            var sqlda = new SqlCommand("UPDATE Player SET age=@age, height=@height, weight=@weight, vertical=@vertical WHERE playerID=@playerID", App.sqlcon);
            sqlda.Parameters.AddWithValue("age", profile.age);
            sqlda.Parameters.AddWithValue("height", profile.height);
            sqlda.Parameters.AddWithValue("weight", profile.weight);
            sqlda.Parameters.AddWithValue("vertical", profile.vertical);
            sqlda.Parameters.AddWithValue("playerID", App.playerID);
            sqlda.ExecuteNonQuery();

            App.sqlcon.Close();
        }

    }
}
