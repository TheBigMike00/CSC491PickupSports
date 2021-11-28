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
            friendList = new List<FriendList>();
            game = new List<Game>();
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            //Load Data for Friends Page
            #region InitializeFriendsPage
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT player1ID, player2ID FROM Friendship WHERE player1ID=@player1ID OR player2ID=@player2ID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("player1ID", App.playerID);
            sqlda.SelectCommand.Parameters.AddWithValue("player2ID", App.playerID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                SqlDataAdapter sqlda2 = new SqlDataAdapter("SELECT profileName, profilePic FROM Player WHERE playerID=@playerID", App.sqlcon);
                DataTable dtbl2 = new DataTable();
                string temp1, temp2;
                try { temp1 = dtbl.Rows[i]["player1ID"].ToString(); }
                catch (Exception e)
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

                    friendList.Add(new FriendList()
                    {
                        profileName = dtbl2.Rows[0]["profileName"].ToString(),
                        profilePic = dtbl2.Rows[0]["profilePic"].ToString()
                    });
                }
                if (temp2 != null && Guid.Parse(temp2) != App.playerID)
                {
                    sqlda2.SelectCommand.Parameters.AddWithValue("playerID", Guid.Parse(temp2));
                    sqlda2.Fill(dtbl2);

                    friendList.Add(new FriendList()
                    {
                        profileName = dtbl2.Rows[0]["profileName"].ToString(),
                        profilePic = dtbl2.Rows[0]["profilePic"].ToString()
                    });
                }
            }
            numFriends = friendList.Count;
            #endregion


            //Load Data for Teams Page
            #region InitializeTeamsPage
            sqlda = new SqlDataAdapter("SELECT teamID FROM Player WHERE playerID=@playerID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
            dtbl = new DataTable();
            sqlda.Fill(dtbl);

            Guid? teamId;
            try 
            {
                teamId = Guid.Parse(dtbl.Rows[0]["teamID"].ToString());
            }
            catch(Exception e)
            {
                string error = e.ToString();
                teamId = null;
            }

            if(teamId != null)
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT teamName, members, wins, losses FROM Team WHERE teamID=@teamID", App.sqlcon);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("teamID", teamId);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                teamName = dataTable.Rows[0]["teamName"].ToString();
                members = dataTable.Rows[0]["members"].ToString() + " Members";
                record = "Record: " + dataTable.Rows[0]["wins"].ToString() + "-" + dataTable.Rows[0]["losses"].ToString();


                sqlda = new SqlDataAdapter("SELECT * FROM Game WHERE team1ID=@team1ID OR team2ID=@team2ID ORDER BY date DESC", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("team1ID", teamId);
                sqlda.SelectCommand.Parameters.AddWithValue("team2ID", teamId);
                dtbl = new DataTable();
                sqlda.Fill(dtbl);


                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    SqlDataAdapter sqlda1 = new SqlDataAdapter("SELECT teamName FROM Team WHERE teamID=@teamID", App.sqlcon);
                    sqlda1.SelectCommand.Parameters.AddWithValue("teamID", dtbl.Rows[i]["team1ID"].ToString());
                    DataTable dtbl1 = new DataTable();
                    sqlda1.Fill(dtbl1);

                    SqlDataAdapter sqlda2 = new SqlDataAdapter("SELECT teamName FROM Team WHERE teamID=@teamID", App.sqlcon);
                    sqlda2.SelectCommand.Parameters.AddWithValue("teamID", dtbl.Rows[i]["team2ID"].ToString());
                    DataTable dtbl2 = new DataTable();
                    sqlda2.Fill(dtbl2);

                    string gameOutcome;
                    int t1 = Convert.ToInt32(dtbl.Rows[i]["team1Score"].ToString());
                    int t2 = Convert.ToInt32(dtbl.Rows[i]["team2Score"].ToString());
                    if ((teamId == Guid.Parse(dtbl.Rows[i]["team1ID"].ToString()) && t1 > t2) || (teamId == Guid.Parse(dtbl.Rows[i]["team2ID"].ToString()) && t2 > t1))
                    {
                        gameOutcome = "VICTORY";
                    }
                    else if (t1 == t2)
                    {
                        gameOutcome = "DRAW";
                    }
                    else
                    {
                        gameOutcome = "LOSS";
                    }

                    game.Add(new Game()
                    {
                        timeDate = formatTime(Convert.ToInt32(dtbl.Rows[i]["time"].ToString())) + " " + formatDate(dtbl.Rows[i]["date"].ToString()),
                        gameLocation = dtbl.Rows[i]["location"].ToString(),
                        team1Name = dtbl1.Rows[0]["teamName"].ToString(),
                        team2Name = dtbl2.Rows[0]["teamName"].ToString(),
                        team1Score = t1,
                        team2Score = t2,
                        outcome = gameOutcome
                    });
                }
            }
            #endregion

            App.sqlcon.Close();


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

        //Friends Content Page
        #region Friends

        string searchVal;
        public string search { set => SetProperty(ref searchVal, value); }

        int numFriendsVal;
        public int numFriends { get => numFriendsVal; set => SetProperty(ref numFriendsVal, value); }

        public List<FriendList> friendList { get; set; }

        public Command SearchCommand { get; }

        public Command RemoveFriendCommand { get; }
        #endregion


        //Teams Content Page
        #region Teams
        string teamNameVal;
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        string membersVal;
        public string members { get => membersVal; set => SetProperty(ref membersVal, value); }

        string recordVal;
        public string record { get => recordVal; set => SetProperty(ref recordVal, value); }

        public List<Game> game {get; set; }

        private string formatTime(int mins)
        {
            int hours, minutes;
            string hoursToDisplay, minutesToDisplay, ampm;

            if (mins < 60)
            {
                hoursToDisplay = "12";
                minutesToDisplay = mins.ToString();
                if (minutesToDisplay.Length == 1)
                    minutesToDisplay = "0" + minutesToDisplay;
                ampm = "am";
                return hoursToDisplay + ":" + minutesToDisplay + ampm;
            }
            else
            {
                hours = mins / 60;
                minutes = mins - (hours * 60);
                minutesToDisplay = minutes.ToString();
            }

            if(hours > 12)
            {
                hoursToDisplay = (hours - 12).ToString();
                ampm = "pm";
            }
            else
            {
                hoursToDisplay = hours.ToString();
                ampm = "am";
            }

            if (minutesToDisplay.Length == 1)
                minutesToDisplay = "0" + minutesToDisplay;
            return hoursToDisplay + ":" + minutesToDisplay + ampm;
        }

        private string formatDate(string date)
        {
            string dateToReturn = "";

            for (int i = 0; i<date.Length; i++)
            {
                if(date[i] == ' ')
                {
                    return dateToReturn;
                }
                dateToReturn = dateToReturn + date[i];
            }
            return dateToReturn;
        }


        public Command EditTeamDetails { get; }
        #endregion

    }
}
