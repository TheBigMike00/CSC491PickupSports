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
            //MSSQLSERVER
            //LAPTOP-NS5R2J8I
            //192.168.1.9 wifi
            //192.168.56.1 lan
            try
            {
                App.sqlcon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("Select * from Player", App.sqlcon);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);
                profileName = dtbl.Rows[2]["profileName"].ToString();
                name = dtbl.Rows[2]["firstName"].ToString() + "  " + dtbl.Rows[2]["lastName"].ToString();
                age = Convert.ToInt32(dtbl.Rows[2]["age"].ToString());
                height = convertInches(Convert.ToInt32(dtbl.Rows[2]["height"].ToString()));
                weight = Convert.ToInt32(dtbl.Rows[2]["weight"].ToString());
                vertical = Convert.ToInt32(dtbl.Rows[2]["vertical"].ToString());
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

        string profileNameVal = "Test";
        public string profileName { get => profileNameVal; set => SetProperty(ref profileNameVal, value); }

        string nameVal = "Phil Collins";
        public string name { get => nameVal; set => SetProperty(ref nameVal, value); }

        string teamNameVal = "Ballers";
        public string teamName { get => teamNameVal; set => SetProperty(ref teamNameVal, value); }

        int ageVal = 23;
        public int age { get => ageVal; set => SetProperty(ref ageVal, value); }

        private string convertInches(int inches)
        {
            string feet = (inches / 12).ToString();
            string inchesToReturn = (inches % 12).ToString();
            return feet + "'" + inchesToReturn + "\"";
        }
        string heightVal = "6'3\"";
        public string height { get => heightVal; set => SetProperty(ref heightVal, value); }

        int weightVal = 230;
        public int weight { get => weightVal; set => SetProperty(ref weightVal, value); }

        int verticalVal = 17;
        public int vertical { get => verticalVal; set => SetProperty(ref verticalVal, value); }

    }
}
