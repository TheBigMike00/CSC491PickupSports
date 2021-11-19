using PickupSports.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                App.sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * from Player WHERE profileName=@profileName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("profileName", profileName);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                if (password == dtbl.Rows[0]["password"].ToString())
                {
                    App.playerID = (Guid)dtbl.Rows[0]["playerID"];
                    App.sqlcon.Close();
                
                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//CommunityPage");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }
        }

        string profileNameVal;
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        string passwordVal;
        public string password { get => passwordVal; set => SetProperty(ref passwordVal, value); }
    }
}