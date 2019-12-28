using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaApi.Business.Abstract;
using InstaApi.Entities.Concrate;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;

namespace InstaApi.Business.Concrate
{
    public class MessageManeger:IMessageManeger
    {
        public async Task<List<EMessageBox>> GetMessageBox(IInstaApi _instaApi)
        {
            var _messageFriends = new List<EMessageBox>();
            var rankedRecipients = await _instaApi.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
            var threads = rankedRecipients.Value.Inbox.Threads;

            var pendingDirect = await _instaApi.MessagingProcessor
                .GetPendingDirectAsync(PaginationParameters.MaxPagesToLoad(1));

            foreach (var instaRankedRecipientThread in threads)
            {
                _messageFriends.Add(new EMessageBox()
                {
                    IsPeddingUser = false,
                    UserName = instaRankedRecipientThread.Title
                });
            }

            if (pendingDirect.Value.Inbox.Threads != null)
            {
                foreach (var pendingUser in pendingDirect.Value.Inbox.Threads)
                {
                    _messageFriends.Add(new EMessageBox()
                    {
                        IsPeddingUser = true,
                        UserName = pendingUser.Title
                    });
                }
            }

            return _messageFriends;
        }

        public async Task<List<EMessage>> GetMessageByUserName(IInstaApi _instaApi, string userName, bool isPendingUser)
        {
            List<EMessage> getMessageList = new List<EMessage>();

            InstagramApiSharp.Classes.IResult<InstaDirectInboxContainer> inbox;
            if (isPendingUser)
            {
                inbox = await _instaApi.MessagingProcessor.GetPendingDirectAsync(PaginationParameters.MaxPagesToLoad(1));
            }
            else
            {
                inbox = await _instaApi.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
            }
            var desireUsername = userName;
            var desireThread = inbox.Value.Inbox.Threads
                .Find(u => u.Users.FirstOrDefault().UserName.ToLower() == desireUsername);
            var requestedThreadId = desireThread.ThreadId;

            if (isPendingUser)
                await _instaApi.MessagingProcessor.ApproveDirectPendingRequestAsync(requestedThreadId);





            var threads = await _instaApi.MessagingProcessor
                .GetDirectInboxThreadAsync(requestedThreadId, PaginationParameters.MaxPagesToLoad(1));



            foreach (var instaDirectInboxItem in threads.Value.Items)
            {
                var a = _instaApi.GetLoggedUser();

                if (instaDirectInboxItem.UserId == a.LoggedInUser.Pk)
                {
                    getMessageList.Add(new EMessage() { Message = instaDirectInboxItem.Text, IsSendedMessage = true });
                }
                else
                    getMessageList.Add(new EMessage() { Message = instaDirectInboxItem.Text, IsSendedMessage = false });
            }

            return getMessageList;
        }

        public async Task<bool> SendMessageByUsername(IInstaApi _instaApi, string userName, string text)
        {
            var desireUsername = userName;
            var user = await _instaApi.UserProcessor.GetUserAsync(desireUsername);
            var userId = user.Value.Pk.ToString();
           IResult<InstaDirectInboxThreadList> result = await _instaApi.MessagingProcessor
                .SendDirectTextAsync(userId, null, text);
           return result.Succeeded;
        }
    }
}
