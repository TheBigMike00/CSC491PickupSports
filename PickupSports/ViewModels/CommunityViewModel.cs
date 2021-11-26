using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using PickupSports.Models;
using PickupSports.Views;
using Xamarin.Forms;

namespace PickupSports.ViewModels
{
    class CommunityViewModel : BaseViewModel
    {
        public CommunityViewModel()
        {
            postFeed = new List<CommunityFeed>();

            if (App.sqlcon.State == ConnectionState.Closed)
                App.sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM Post ORDER BY createdTime DESC", App.sqlcon);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                sqlda = new SqlDataAdapter("SELECT profilePic, profileName FROM Player WHERE playerID=@playerID", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("playerID", dtbl.Rows[i]["playerID"].ToString());
                DataTable dtbl2 = new DataTable();
                sqlda.Fill(dtbl2);

                postFeed.Add(new CommunityFeed()
                {
                    profilePicVal = dtbl2.Rows[0]["profilePic"].ToString(),
                    profileNameVal = dtbl2.Rows[0]["profileName"].ToString(),
                    imageSourceVal = dtbl.Rows[i]["source"].ToString(),
                    captionVal = dtbl.Rows[i]["caption"].ToString(),
                    postDateVal = dtbl.Rows[i]["createdTime"].ToString()
                });
            }
            App.sqlcon.Close();
        }

        public List<CommunityFeed> postFeed { get; set; }
    }
}