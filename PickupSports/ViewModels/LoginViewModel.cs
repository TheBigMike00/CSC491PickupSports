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
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            CreateAccountCommand = new Command(OnCreateAccountClicked);
        }

        private async void OnCreateAccountClicked(object obj)
        {
            try 
            {
                if (App.sqlcon.State == ConnectionState.Closed)
                    App.sqlcon.Open();
                App.playerID = Guid.NewGuid();
                var sqlda = new SqlCommand("INSERT INTO Player (playerID, profileName, password, firstName, lastName, age, height, weight, vertical) VALUES (@playerID, @profileName, @password, @firstName, @lastName, @age, @height, @weight, @vertical)", App.sqlcon);
                sqlda.Parameters.AddWithValue("playerID", App.playerID);
                sqlda.Parameters.AddWithValue("profileName", profileName);
                sqlda.Parameters.AddWithValue("password", password);
                sqlda.Parameters.AddWithValue("firstName", firstName);
                sqlda.Parameters.AddWithValue("lastName", lastName);
                sqlda.Parameters.AddWithValue("age", age);
                sqlda.Parameters.AddWithValue("height", height);
                sqlda.Parameters.AddWithValue("weight", weight);
                sqlda.Parameters.AddWithValue("vertical", vertical);
                sqlda.ExecuteNonQuery();

                App.sqlcon.Close();

                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("//CommunityPage");
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                App.sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * from Player WHERE profileName=@profileName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("profileName", profileNameLogin);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                if (passwordLogin == dtbl.Rows[0]["password"].ToString())
                {
                    App.playerID = (Guid)dtbl.Rows[0]["playerID"];
                    App.sqlcon.Close();

                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//CommunityPage");
                }
                else
                    App.sqlcon.Close();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }
        }

        public Command LoginCommand { get; }
        public Command CreateAccountCommand { get; }

        //Login Fields
        string profileNameLoginVal;
        public string profileNameLogin { get => profileNameLoginVal; set => SetProperty(ref profileNameLoginVal, value); }

        string passwordLoginVal;
        public string passwordLogin { get => passwordLoginVal; set => SetProperty(ref passwordLoginVal, value); }


        //Create Account Fields
        string profileNameVal;
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        string passwordVal;
        public string password { get => passwordVal; set => SetProperty(ref passwordVal, value); }

        string firstNameVal;
        public string firstName { get => firstNameVal; set => SetProperty(ref firstNameVal, value); }

        string lastNameVal;
        public string lastName { get => lastNameVal; set => SetProperty(ref lastNameVal, value); }

        int? ageVal;
        public int? age { get => ageVal; set => SetProperty(ref ageVal, value); }

        string heightVal;
        public string height { get => heightVal; set => SetProperty(ref heightVal, value); }

        int? weightVal;
        public int? weight { get => weightVal; set => SetProperty(ref weightVal, value); }

        int? verticalVal;
        public int? vertical { get => verticalVal; set => SetProperty(ref verticalVal, value); }
    }
}