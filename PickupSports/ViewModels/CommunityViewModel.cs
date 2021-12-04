using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PickupSports.Models;
using PickupSports.Views;
using Xamarin.CommunityToolkit.ObjectModel;
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
            
            ViewProfile = new AsyncCommand<object>(ViewProfileComm);
        }

        async Task ViewProfileComm(object args)
        {
            try
            {
                SelectedPost = args as CommunityFeed;
                if (App.sqlcon.State == ConnectionState.Closed)
                    App.sqlcon.Open();

                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT playerID FROM Player WHERE profileName=@profileName", App.sqlcon);
                sqlda.SelectCommand.Parameters.AddWithValue("profileName", SelectedPost.profileNameVal);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                App.tempPlayerID = Guid.Parse(dtbl.Rows[0]["playerID"].ToString());
                App.sqlcon.Close();
                SelectedPost = null;

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

        public List<CommunityFeed> postFeed { get; set; }

        CommunityFeed selectedPostVal;
        public CommunityFeed SelectedPost { get => selectedPostVal; set => SetProperty(ref selectedPostVal, value); }

        public AsyncCommand <object>ViewProfile { get; }
    }
}