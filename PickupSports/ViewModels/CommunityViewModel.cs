using System;
using System.Collections.Generic;
using System.Text;
using PickupSports.Models;
using PickupSports.Views;

namespace PickupSports.ViewModels
{
	public class Data
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string Comment { get; set; }
		public int CommentCount { get; set; }
		public string PostedAt { get; set; }
		public int ViewCount { get; set; }
	}

	class CommunityViewModel : BaseViewModel
    {
		public CommunityViewModel()
        {
            Title = "Community";
		}
    }
}