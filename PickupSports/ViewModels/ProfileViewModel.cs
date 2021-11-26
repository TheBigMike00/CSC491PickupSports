using PickupSports.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PickupSports.ViewModels
{
    class ProfileViewModel : BaseViewModel
    {
        public ProfileViewModel()
        {
            try
            {
                if(App.sqlcon.State == ConnectionState.Closed)
                    App.sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM Player WHERE playerID=@playerID", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                profilePic = dtbl.Rows[0]["profilePic"].ToString();
                profileName = dtbl.Rows[0]["profileName"].ToString();
                name = dtbl.Rows[0]["firstName"].ToString() + "  " + dtbl.Rows[0]["lastName"].ToString();
                age = Convert.ToInt32(dtbl.Rows[0]["age"].ToString());
                height = convertInches(Convert.ToInt32(dtbl.Rows[0]["height"].ToString()));
                weight = Convert.ToInt32(dtbl.Rows[0]["weight"].ToString());
                vertical = Convert.ToInt32(dtbl.Rows[0]["vertical"].ToString());


                string output = dtbl.Rows[0]["teamID"].ToString();
                if (dtbl.Rows[0]["teamID"].ToString() == "")
                    teamName = "No Team";
                else
                {
                    sqlda = new SqlDataAdapter("SELECT teamName FROM Team WHERE teamID=@teamID", App.sqlcon);
                    sqlda.SelectCommand.Parameters.AddWithValue("teamID", Guid.Parse(dtbl.Rows[0]["teamID"].ToString()));
                    dtbl = new DataTable();
                    sqlda.Fill(dtbl);
                    teamName = dtbl.Rows[0]["teamName"].ToString();
                }
                

                sqlda = new SqlDataAdapter("SELECT * FROM Friendship WHERE player1ID=@player1ID OR player2ID=@player2ID", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("player1ID", App.playerID);
                sqlda.SelectCommand.Parameters.AddWithValue("player2ID", App.playerID);
                dtbl = new DataTable();
                sqlda.Fill(dtbl);
                friends = dtbl.Rows.Count.ToString() + " Friends";


                profileFeed = new List<CommunityFeed>();
                sqlda = new SqlDataAdapter("SELECT * FROM Post WHERE playerID=@playerID ORDER BY createdTime DESC", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("playerID", App.playerID);
                dtbl = new DataTable();
                sqlda.Fill(dtbl);
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    profileFeed.Add(new CommunityFeed()
                    {
                        profilePicVal = profilePic,
                        profileNameVal = profileName,
                        imageSourceVal = dtbl.Rows[i]["source"].ToString(),
                        captionVal = dtbl.Rows[i]["caption"].ToString(),
                        postDateVal = dtbl.Rows[i]["createdTime"].ToString()
                    });
                }


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

        public List<CommunityFeed> profileFeed { get; set; }

        string profilePicVal;
        public string profilePic { get => profilePicVal; set => SetProperty(ref profilePicVal, value); }

        string profileNameVal;
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        string nameVal;
        public string name { get => nameVal; set => SetProperty(ref nameVal, value); }

        string teamNameVal;
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        string friendsVal;
        public string friends { get => friendsVal; set => SetProperty(ref friendsVal, value); }

        int ageVal;
        public int age { get => ageVal; set => SetProperty(ref ageVal, value); }

        private string convertInches(int inches)
        {
            string feet = (inches / 12).ToString();
            string inchesToReturn = (inches % 12).ToString();
            return feet + "'" + inchesToReturn + "\"";
        }
        string heightVal;
        public string height { get => heightVal; set => SetProperty(ref heightVal, value); }

        int weightVal;
        public int weight { get => weightVal; set => SetProperty(ref weightVal, value); }

        int verticalVal;
        public int vertical { get => verticalVal; set => SetProperty(ref verticalVal, value); }

    }
}
