using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaApi.Business.Abstract;
using InstaApi.Entities.Concrate;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;

namespace InstaApi.Business.Concrate
{
    public class LoginManeger:ILoginManeger
    {
        public async Task LoginInsta(string userName, string password)
        {
            var userSession = new UserSessionData
            {
                UserName = userName,
                Password = password
            };

            var _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            string stateFile = "state" + userName + ".bin";
            ;
            try
            {
                // load session file if exists
                if (File.Exists(stateFile))
                {
                    Console.WriteLine("Loading state from file");
                    using (var fs = File.OpenRead(stateFile))
                    {
                        _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                        // in .net core or uwp apps don't use LoadStateDataFromStream
                        // use this one:
                        // _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                        // you should pass json string as parameter to this function.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (!_instaApi.IsUserAuthenticated)
            {
                // login
                Console.WriteLine($"Logging in as {userSession.UserName}");
                var logInResult = await _instaApi.LoginAsync();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");

                 

                    if (logInResult.Value == InstaLoginResult.ChallengeRequired)
                    {
                        return;
                    }
                    else if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            // save session in file
            var state = _instaApi.GetStateDataAsStream();
            // in .net core or uwp apps don't use GetStateDataAsStream.
            // use this one:
            // var state = _instaApi.GetStateDataAsString();
            // this returns you session as json string.
            using (var fileStream = File.Create(stateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }

        public Task SingOut(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EInstaUser>> LoginedUsers()
        {
            string[] binFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.bin")
                .Select(Path.GetFileName)
                .ToArray();
            foreach (var file in binFiles)
            {

                if (file.Substring(0, 5) == "state")
                {
                    string instaUser = file.Remove(file.Length - 4).Remove(0, 5);
                    AddOnlineUserInStateFile(instaUser);
                }
            }

            return EInstaUser.instaUsers;
        }

        public void AddOnlineUserInStateFile(string username)
        {
            EInstaUser instaUser = null;
          
            if (EInstaUser.instaUsers != null && EInstaUser.instaUsers.Any())
            {
                instaUser = EInstaUser.instaUsers.SingleOrDefault(x => x.UserName == username);
            }


            if (instaUser == null)
            {
                var userSession = new UserSessionData
                {
                    UserName = username,
                    Password = ""
                };

                var _instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();

                string stateFile = "state" + username + ".bin";
                ;
                try
                {
                    // load session file if exists
                    if (File.Exists(stateFile))
                    {
                        Console.WriteLine("Loading state from file");
                        using (var fs = File.OpenRead(stateFile))
                        {
                            _instaApi.LoadStateDataFromStream(fs);
                            // in .net core or uwp apps don't use LoadStateDataFromStream
                            // use this one:
                            // _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                            // you should pass json string as parameter to this function.
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (_instaApi.IsUserAuthenticated)
                {
                    EInstaUser.instaUsers.Add(new EInstaUser() { UserName = username, Password = "*****", _instaApi = _instaApi });

                }
                else
                {
                    var itemToRemove = EInstaUser.instaUsers.SingleOrDefault(r => r.UserName == username);
                    if (itemToRemove != null)
                        EInstaUser.instaUsers.Remove(itemToRemove);
                }

            }

        }

    }
    
}
