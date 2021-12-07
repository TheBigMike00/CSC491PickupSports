using PickupSports.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    class AddGameViewModel:BaseViewModel
    {
        public HooperViewModel hvm;

        public AddGameViewModel()
        {

        }

        public AddGameViewModel(HooperViewModel hooperVM)
        {
            hvm = hooperVM;
            teamName = hvm.teamName;


            Save = new Command(async () =>
            {
                SaveGame();
                await App.Current.MainPage.Navigation.PopAsync();

            });


            Cancel = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        async void SaveGame()
        {
            try
            {
                if (App.sqlcon.State == ConnectionState.Closed)
                    App.sqlcon.Open();


                Guid yourTeamID, opTeamID;
                int yourWins, yourLosses, opWins, opLosses;

                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT teamID, wins, losses FROM Team WHERE teamName=@teamName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("teamName", teamName);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                yourTeamID = Guid.Parse(dtbl.Rows[0]["teamID"].ToString());
                yourWins = Convert.ToInt32(dtbl.Rows[0]["wins"].ToString());
                yourLosses = Convert.ToInt32(dtbl.Rows[0]["losses"].ToString());

                sqlda = new SqlDataAdapter("SELECT teamID, wins, losses FROM Team WHERE teamName=@teamName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("teamName", opTeamName);
                dtbl = new DataTable();
                sqlda.Fill(dtbl);
                opTeamID = Guid.Parse(dtbl.Rows[0]["teamID"].ToString());
                opWins = Convert.ToInt32(dtbl.Rows[0]["wins"].ToString());
                opLosses = Convert.ToInt32(dtbl.Rows[0]["losses"].ToString());


                var sqlda2 = new SqlCommand("INSERT INTO Game (gameID, team1ID, team2ID, time, date, location, team1Score, team2Score) VALUES (@gameID, @team1ID, @team2ID, @time, @date, @location, @team1Score, @team2Score)", App.sqlcon);
                sqlda2.Parameters.AddWithValue("gameID", Guid.NewGuid());
                sqlda2.Parameters.AddWithValue("team1ID", yourTeamID);
                sqlda2.Parameters.AddWithValue("team2ID", opTeamID);
                sqlda2.Parameters.AddWithValue("time", getMinsAfterMidnight(time));
                sqlda2.Parameters.AddWithValue("date", date);
                sqlda2.Parameters.AddWithValue("location", location);
                sqlda2.Parameters.AddWithValue("team1Score", yourScore);
                sqlda2.Parameters.AddWithValue("team2Score", opScore);
                sqlda2.ExecuteNonQuery();

                sqlda2 = new SqlCommand("UPDATE Team SET wins = @wins, losses = @losses WHERE teamID = @teamID", App.sqlcon);
                if(yourScore > opScore)
                {
                    sqlda2.Parameters.AddWithValue("wins", yourWins + 1);
                    sqlda2.Parameters.AddWithValue("losses", yourLosses);
                }
                else if(yourScore < opScore)
                {
                    sqlda2.Parameters.AddWithValue("wins", yourWins);
                    sqlda2.Parameters.AddWithValue("losses", yourLosses + 1);
                }
                else
                {
                    sqlda2.Parameters.AddWithValue("wins", yourWins);
                    sqlda2.Parameters.AddWithValue("losses", yourLosses);
                }
                sqlda2.Parameters.AddWithValue("teamID", yourTeamID);
                sqlda2.ExecuteNonQuery();


                sqlda2 = new SqlCommand("UPDATE Team SET wins = @wins, losses = @losses WHERE teamID = @teamID", App.sqlcon);
                if (yourScore > opScore)
                {
                    sqlda2.Parameters.AddWithValue("wins", opWins + 1);
                    sqlda2.Parameters.AddWithValue("losses", opLosses);
                }
                else if (yourScore < opScore)
                {
                    sqlda2.Parameters.AddWithValue("wins", opWins);
                    sqlda2.Parameters.AddWithValue("losses", opLosses + 1);
                }
                else
                {
                    sqlda2.Parameters.AddWithValue("wins", opWins);
                    sqlda2.Parameters.AddWithValue("losses", opLosses);
                }
                sqlda2.Parameters.AddWithValue("teamID", opTeamID);
                sqlda2.ExecuteNonQuery();

                App.sqlcon.Close();

                hvm.game.Clear();
                hvm.LoadTeamData();
            }
            catch(Exception e)
            {
                string error = e.ToString();
                await App.Current.MainPage.DisplayAlert("Error", "Unable to save this game\n\nPlease ensure all fields are populated and valid.", "OK");
            }        
        }

        int getMinsAfterMidnight(TimeSpan time)
        {
            return (time.Hours * 60) + (time.Minutes);
        }


        string yourTeamNameVal;
        public string teamName { get => yourTeamNameVal; set => SetProperty(ref yourTeamNameVal, value); }

        int yourScoreVal;
        public int yourScore { get => yourScoreVal; set => SetProperty(ref yourScoreVal, value); }

        string opTeamNameVal;
        public string opTeamName { get => opTeamNameVal; set => SetProperty(ref opTeamNameVal, value); }

        int opScoreVal;
        public int opScore { get => opScoreVal; set => SetProperty(ref opScoreVal, value); }

        string locationVal;
        public string location { get => locationVal; set => SetProperty(ref locationVal, value); }

        TimeSpan timeVal;
        public TimeSpan time { get => timeVal; set => SetProperty(ref timeVal, value); }

        DateTime dateVal;
        public DateTime date { get => dateVal; set => SetProperty(ref dateVal, value); }

        DateTime maxDateVal = DateTime.Today;
        public DateTime maxDate { get => maxDateVal; set => SetProperty(ref maxDateVal, value); }

        DateTime minDateVal = DateTime.Today.AddMonths(-1);
        public DateTime minDate { get => minDateVal; set => SetProperty(ref minDateVal, value); }

        public Command Save { get; }

        public Command Cancel { get; }

    }
}
