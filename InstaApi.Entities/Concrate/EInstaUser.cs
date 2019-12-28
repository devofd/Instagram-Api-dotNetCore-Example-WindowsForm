using System;
using System.Collections.Generic;
using System.Text;
using InstagramApiSharp.API;

namespace InstaApi.Entities.Concrate
{
    public class EInstaUser
    {
        public static List<EInstaUser> instaUsers = new List<EInstaUser>();
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public IInstaApi _instaApi { get; set; }
    }
}
