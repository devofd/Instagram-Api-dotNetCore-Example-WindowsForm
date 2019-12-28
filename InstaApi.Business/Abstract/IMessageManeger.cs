using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InstaApi.Entities.Concrate;
using InstagramApiSharp.API;

namespace InstaApi.Business.Abstract
{
    public interface IMessageManeger
    {

        Task<List<EMessageBox>> GetMessageBox(IInstaApi _instaApi);

        Task<List<EMessage>> GetMessageByUserName(IInstaApi _instaApi, string userName, bool isPendingUser);

        Task<bool> SendMessageByUsername(IInstaApi _instaApi, string userName, string text);

    }
}
