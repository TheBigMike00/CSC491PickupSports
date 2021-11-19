using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    class HooperViewModel : BaseViewModel
    {
        public HooperViewModel()
        {

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

        //Friends Content Page
        #region Friends
        string searchVal;
        public string search { set => SetProperty(ref searchVal, value); }


        int numFriendsVal = 1;
        public int numFriends { get => numFriendsVal; set => SetProperty(ref numFriendsVal, value); }


        string profileNameVal = "John.Doe3";
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        public Command SearchCommand { get; }

        public Command RemoveFriendCommand { get; }
        #endregion


        //Teams Content Page
        #region Teams
        string teamNameVal = "Ballers";
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        string membersVal = "9 Members";
        public string members { get => membersVal; set => SetProperty(ref membersVal, value); }

        string recordVal = "Record: 5-2";
        public string record { get => recordVal; set => SetProperty(ref recordVal, value); }

        string timeDateVal = "7:30pm 10/21/2021";
        public string timeDate { get => timeDateVal; set => SetProperty(ref timeDateVal, value); }

        string gameLocationVal = "@ Concordia University";
        public string gameLocation { get => gameLocationVal; set => SetProperty(ref gameLocationVal, value); }

        string team1NameVal = "Ballers";
        public string team1Name { get => team1NameVal; set => SetProperty(ref team1NameVal, value); }

        string team2NameVal = "Tigers";
        public string team2Name { get => team2NameVal; set => SetProperty(ref team2NameVal, value); }

        int team1ScoreVal = 54;
        public int team1Score { get => team1ScoreVal; set => SetProperty(ref team1ScoreVal, value); }

        int team2ScoreVal = 63;
        public int team2Score { get => team2ScoreVal; set => SetProperty(ref team2ScoreVal, value); }

        string outcomeVal = "LOSS";
        public string outcome { get => outcomeVal; set => SetProperty(ref outcomeVal, value); }

        public Command EditTeamDetails { get; }
        #endregion

    }
}
