using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InstaApi.Entities.Concrate;


namespace InstaApi.Business.Abstract
{
    public interface ILoginManeger
    {

        Task LoginInsta(string userName, string password);

        Task SingOut (string userName);

        Task<List<EInstaUser>> LoginedUsers();
    }
}
