using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkUser
    {
        public BkUser()
        {
            BkBarrage = new HashSet<BkBarrage>();
            BkBrowseHistory = new HashSet<BkBrowseHistory>();
            BkCollection = new HashSet<BkCollection>();
            BkComments = new HashSet<BkComments>();
            BkFavouriteFavUser = new HashSet<BkFavourite>();
            BkFavouriteU = new HashSet<BkFavourite>();
            BkInteractiveVideo = new HashSet<BkInteractiveVideo>();
        }

        public int UId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual BkUserInfo BkUserInfo { get; set; }
        public virtual ICollection<BkBarrage> BkBarrage { get; set; }
        public virtual ICollection<BkBrowseHistory> BkBrowseHistory { get; set; }
        public virtual ICollection<BkCollection> BkCollection { get; set; }
        public virtual ICollection<BkComments> BkComments { get; set; }
        public virtual ICollection<BkFavourite> BkFavouriteFavUser { get; set; }
        public virtual ICollection<BkFavourite> BkFavouriteU { get; set; }
        public virtual ICollection<BkInteractiveVideo> BkInteractiveVideo { get; set; }
    }
}
