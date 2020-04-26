using MiniMessengerServer.Data;
using System;
using System.Collections.Specialized;
using System.Text;

namespace MiniMessengerServer
{
    internal class CommonHelper
    {
        private static readonly DataManager _dataManager = new DataManager();
        private static readonly DeviceManager _deviceManager = new DeviceManager();
        private int _userId = 0;

        internal long CreateUserId()
        {
            this._userId++;
            return this._userId;
        }

        private long _messageId = 0;
        internal long CreateMessageId()
        {
            this._messageId++;
            return this._messageId;
        }

        internal static Response GetOnlineUser(NameValueCollection queryString)
        {
            if(long.TryParse(queryString.Get("userid"), out long userId))
            {
                _dataManager.UpdateUserAlive(userId);

                return new Response
                {
                    Success = true,
                    Content = _dataManager.GetUsers(userId)
                };
            }

            SimpleLogging.Log($"GetOnlineUser method has return no data.");
            return Response.CreateFail("No data");
        }

        internal static Response SendCommand(NameValueCollection arg)
        {
            if(long.TryParse(arg.Get("deviceid"), out long deviceId))
            {
                
            }


            return Response.CreateFail("no device");
        }

        internal static Response GetMessages(NameValueCollection queryString)
        {
            if (long.TryParse(queryString.Get("userid"), out long userId) &&
                long.TryParse(queryString.Get("touserid"), out long toUserId))
            {
                return new Response
                {
                    Success = true,
                    Content = _dataManager.GetMessages(userId, toUserId)
                };
            }

            var sb = new StringBuilder();
            foreach (var item in queryString.AllKeys)
            {
                sb.Append($"{item}, ");
            }
            SimpleLogging.Log($"userid not valid. Keys: {sb}");
            return Response.CreateFail("ERROR: Userid not valid");
        }

        internal static Response AddUser(NameValueCollection queryString)
        {
            var username = queryString.Get("username");

            return new Response
            {
                Success = true,
                Content = new UserItem[] { _dataManager.AddUser(username) }
            };
        }

        internal static Response SendMessage(NameValueCollection queryString)
        {
            if (long.TryParse(queryString.Get("userid"), out long userId) &&
                long.TryParse(queryString.Get("touserid"), out long toUserid))
            {
                string text = queryString.Get("messagetext");

                // add to self user
                _ = _dataManager.AddMessage(toUserid, userId, text, true);

                return new Response
                {
                    Success = true,
                    Content = _dataManager.AddMessage(userId, toUserid, text)
                };
            }

            return Response.CreateFail("userid not valid");
        }
    }
}