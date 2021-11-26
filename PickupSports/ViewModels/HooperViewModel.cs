using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PickupSports.Models;
using System.Collections.ObjectModel;

namespace PickupSports.ViewModels
{
    class HooperViewModel : BaseViewModel
    {
        public HooperViewModel()
        {
            friendNames = new ObservableCollection<string>();
            //Load Data for Friends Page
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT player1ID, player2ID FROM Friendship WHERE player1ID=@player1ID OR player2ID=@player2ID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("player1ID", App.playerID);
            sqlda.SelectCommand.Parameters.AddWithValue("player2ID", App.playerID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            
            for (int i = 0; i<dtbl.Rows.Count; i++)
            {
                SqlDataAdapter sqlda2 = new SqlDataAdapter("SELECT profileName, profilePic FROM Player WHERE playerID=@playerID", App.sqlcon);
                DataTable dtbl2 = new DataTable();
                string temp1, temp2;
                try{temp1 = dtbl.Rows[i]["player1ID"].ToString();}
                catch(Exception e)
                {
                    string error = e.ToString();
                    temp1 = null;
                }

                try { temp2 = dtbl.Rows[i]["player2ID"].ToString(); }
                catch (Exception e)
                {
                    string error = e.ToString();
                    temp2 = null;
                }


                if (temp1 != null && Guid.Parse(temp1) != App.playerID)
                {
                    sqlda2.SelectCommand.Parameters.AddWithValue("playerID", Guid.Parse(temp1));
                    sqlda2.Fill(dtbl2);
                    //string tempProfileName = dtbl2.Rows[0]["profileName"].ToString();
                    //string tempProfilePicSource = dtbl2.Rows[0]["profilePic"].ToString();
                    friendNames.Add(dtbl2.Rows[0]["profileName"].ToString());
                }
                if (temp2 != null && Guid.Parse(temp2) != App.playerID)
                {
                    sqlda2.SelectCommand.Parameters.AddWithValue("playerID", Guid.Parse(temp2));
                    sqlda2.Fill(dtbl2);
                    friendNames.Add(dtbl2.Rows[0]["profileName"].ToString());
                }                
            }

            numFriends = friendNames.Count;

            
            

            App.sqlcon.Close();


            Title = "Hoopers";

            SearchCommand = new Command(() =>
            {
                
            });

            RemoveFriendCommand = new Command(() =>
            {

            });

            EditTeamDetails = new Command(() =>
            {

            });
        }

        //async Task ExecuteLoadFriendsListCommand()
        //{
        //    IsBusy = true;

        //    try
        //    {
        //        friendNames.Clear();
        //        var items = await DataStore.GetItemsAsync(true);
        //        foreach (var item in items)
        //        {
        //            Items.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //Friends Content Page
        #region Friends
        string searchVal;
        public string search { set => SetProperty(ref searchVal, value); }


        int numFriendsVal;
        public int numFriends { get => numFriendsVal; set => SetProperty(ref numFriendsVal, value); }

        string profileNameVal;
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        public ObservableCollection<string> friendNames { get; }

        public ObservableCollection<string> profilePic { get; }

        public Command SearchCommand { get; }

        public Command RemoveFriendCommand { get; }
        #endregion


        //Teams Content Page
        #region Teams
        string teamNameVal;
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        string membersVal;
        public string members { get => membersVal; set => SetProperty(ref membersVal, value); }

        string recordVal; //= "Record: 5-2";
        public string record { get => recordVal; set => SetProperty(ref recordVal, value); }

        string timeDateVal;// = "7:30pm 10/21/2021";
        public string timeDate { get => timeDateVal; set => SetProperty(ref timeDateVal, value); }

        string gameLocationVal;// = "@ Concordia University";
        public string gameLocation { get => gameLocationVal; set => SetProperty(ref gameLocationVal, value); }

        string team1NameVal;// = "Ballers";
        public string team1Name { get => team1NameVal; set => SetProperty(ref team1NameVal, value); }

        string team2NameVal;// = "Tigers";
        public string team2Name { get => team2NameVal; set => SetProperty(ref team2NameVal, value); }

        int team1ScoreVal;// = 54;
        public int team1Score { get => team1ScoreVal; set => SetProperty(ref team1ScoreVal, value); }

        int team2ScoreVal;// = 63;
        public int team2Score { get => team2ScoreVal; set => SetProperty(ref team2ScoreVal, value); }

        string outcomeVal;// = "LOSS";
        public string outcome { get => outcomeVal; set => SetProperty(ref outcomeVal, value); }

        public Command EditTeamDetails { get; }
        #endregion

    }
}
