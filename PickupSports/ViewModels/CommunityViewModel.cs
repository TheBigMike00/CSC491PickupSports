using System;
using System.Collections.Generic;
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
            postFeed = new List<CommunityFeed>()
            {
                new CommunityFeed()
                {
                    profileNameVal = "profileName5",
                    imageSourceVal = "StockBasketballPhoto1.jpg",
                    commentVal = "This is a comment lol",
                    postDateVal = DateTime.Now.ToString()
                }
            };

            postFeed.Add(new CommunityFeed()
            {
                profileNameVal = "AWEsauce",
                imageSourceVal = "StockBasketballPhoto2.jpg",
                commentVal = "Another Comment",
                postDateVal = DateTime.Now.ToString()
            });
        }

        public List<CommunityFeed> postFeed { get; set; }
    }
}