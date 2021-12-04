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
using Xamarin.CommunityToolkit.ObjectModel;
using PickupSports.Views;

namespace PickupSports.ViewModels
{
    class HooperViewModel : BaseViewModel
    {
        public HooperViewModel()
        {
            friendList = new ObservableCollection<FriendList>();
            game = new List<Game>();

            //Load Data for Friends Page
            LoadFriendData();

            //Load Data for Teams Page
            LoadTeamData();


            ViewProfile = new AsyncCommand<object>(ViewProfileComm);

            SearchCommand = new Command(addFriend);

            RemoveFriendCommand = new Command<string>(removeFriend);

            EditTeamDetails = new Command(() =>
            {

            });

            AddGame = new Command(() =>
            {

            });
        }

        //Friends Content Page
        #region Friends

        void LoadFriendData()
        {
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();
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
            App.sqlcon.Close();
        }

        string searchVal;
        public string search { get => searchVal; set => SetProperty(ref searchVal, value); }

        int numFriendsVal;
        public int numFriends { get => numFriendsVal; set => SetProperty(ref numFriendsVal, value); }

        public ObservableCollection<FriendList> friendList { get; set; }

        FriendList SelectedFriendshipVal;
        public FriendList SelectedFriendship { get => SelectedFriendshipVal; set => SetProperty(ref SelectedFriendshipVal, value); }

        public AsyncCommand<object> ViewProfile { get; }

        public Command SearchCommand { get; }

        public Command <string> RemoveFriendCommand { get; }

        async Task ViewProfileComm(object args)
        {
            try
            {
                SelectedFriendship = args as FriendList;
                if (App.sqlcon.State == ConnectionState.Closed)
                    App.sqlcon.Open();

                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT playerID FROM Player WHERE profileName=@profileName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("profileName", SelectedFriendship.profileName);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                App.tempPlayerID = Guid.Parse(dtbl.Rows[0]["playerID"].ToString());
                App.sqlcon.Close();
                SelectedFriendship = null;

                var viewProfileVM = new ViewProfileViewModel();
                var viewProfilePage = new ViewProfilePage();

                viewProfilePage.BindingContext = viewProfileVM;
                await App.Current.MainPage.Navigation.PushModalAsync(viewProfilePage);
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return;
        }

        void removeFriend(string profileName)
        {
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT playerID FROM Player WHERE profileName=@profileName", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("profileName", profileName);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);

            var sqlda2 = new SqlCommand("DELETE FROM Friendship WHERE (player1ID=@otherID OR player2ID=@otherID) AND (player1ID=@playerID OR player2ID=@playerID)", App.sqlcon);
            sqlda2.Parameters.AddWithValue("otherID", Guid.Parse(dtbl.Rows[0]["playerID"].ToString()));
            sqlda2.Parameters.AddWithValue("playerID", App.playerID);
            sqlda2.ExecuteNonQuery();

            friendList.Clear();
            LoadFriendData();

            App.sqlcon.Close();

        }

        async void addFriend()
        {
            if(search != null)
            {
                bool inList = false;
                for(int i =0; i<friendList.Count; i++)
                {
                    if (friendList[i].profileName == search)
                    {
                        inList = true;
                        break;
                    }
                }

                if(!inList)
                {
                    try
                    {
                        if (App.sqlcon.State == ConnectionState.Closed)
                            App.sqlcon.Open();

                        SqlDataAdapter sqlda = new SqlDataAdapter("SELECT playerID FROM Player WHERE profileName=@profileName", App.sqlcon);
                        sqlda.SelectCommand.Parameters.AddWithValue("profileName", search);
                        DataTable dtbl = new DataTable();
                        sqlda.Fill(dtbl);

                        var sqlda2 = new SqlCommand("INSERT INTO Friendship (friendshipID, player1ID, player2ID) VALUES (@friendshipID, @player1ID, @player2ID)", App.sqlcon);
                        sqlda2.Parameters.AddWithValue("friendshipID", Guid.NewGuid());
                        sqlda2.Parameters.AddWithValue("player1ID", App.playerID);
                        sqlda2.Parameters.AddWithValue("player2ID", Guid.Parse(dtbl.Rows[0]["playerID"].ToString()));
                        sqlda2.ExecuteNonQuery();

                        friendList.Clear();
                        LoadFriendData();

                        App.sqlcon.Close();
                    }
                    catch (Exception e)
                    {
                        string error = e.ToString();
                        await App.Current.MainPage.DisplayAlert("Error", "Check Your Spelling!\n\nNo profile found with provided name.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "This player is already in your friends list.", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter the profile name of the friend you with to add.", "OK");
            }
        }
        #endregion


        //Teams Content Page
        #region Teams
        void LoadTeamData()
        {
            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();

            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT teamID FROM Player WHERE playerID=@playerID", App.sqlcon);
            sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);

            Guid? teamId;
            try
            {
                teamId = Guid.Parse(dtbl.Rows[0]["teamID"].ToString());
            }
            catch (Exception e)
            {
                string error = e.ToString();
                teamId = null;
            }

            if (teamId != null)
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT teamName, members, wins, losses, teamLogo FROM Team WHERE teamID=@teamID", App.sqlcon);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("teamID", teamId);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                teamName = dataTable.Rows[0]["teamName"].ToString();
                members = dataTable.Rows[0]["members"].ToString() + " Members";
                record = "Record: " + dataTable.Rows[0]["wins"].ToString() + "-" + dataTable.Rows[0]["losses"].ToString();
                string logo = dataTable.Rows[0]["teamLogo"].ToString();
                if(logo != null)
                {
                    teamLogo = logo;
                }
                else
                {
                    //null team logo
                }


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
            App.sqlcon.Close();
        }

        string teamNameVal;
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        string membersVal;
        public string members { get => membersVal; set => SetProperty(ref membersVal, value); }

        string recordVal;
        public string record { get => recordVal; set => SetProperty(ref recordVal, value); }

        string teamLogoVal;
        public string teamLogo { get => teamLogoVal; set => SetProperty(ref teamLogoVal, value); }

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

        public Command AddGame { get; }

        public Command EditTeamDetails { get; }
        #endregion

    }
}
