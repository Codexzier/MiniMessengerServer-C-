using MiniMessengerServer.Data;
using System;
using System.Collections.Specialized;
using System.Linq;
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
            if(long.TryParse(queryString.Get("id"), out long userId))
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

        internal static Response GetMessages(NameValueCollection queryString)
        {
            if (long.TryParse(queryString.Get("id"), out long userId) &&
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
            SimpleLogging.Log($"id not valid. Keys: {sb}");
            return Response.CreateFail("ERROR: User id not valid");
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
            if (long.TryParse(queryString.Get("id"), out long userId) &&
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

            return Response.CreateFail("user id not valid");
        }

        internal static Response GetAll(NameValueCollection queryString)
        {
            var devices = _deviceManager.GetDevices();

            if(devices.Any())
            {
                return new Response
                {
                    Success = true,
                    Content = devices
                };
            }

            return Response.CreateFail("no devices registered");
        }

        internal static Response DeviceSendCommand(NameValueCollection queryString)
        {
            if (long.TryParse(queryString.Get("id"), out long deviceId) &&
                long.TryParse(queryString.Get("value"), out long deviceValue))
            {

                var success = _deviceManager.SendCommand(deviceId, deviceValue, out string message) as bool?;

                if(success == null)
                {
                    return Response.CreateFail("no device");
                }

                return new Response
                {
                    Success = success.Value,
                    Content = message
                };
            }

            return Response.CreateFail("check parameter for device id and value");
        }

        internal static Response DeviceGetValue(NameValueCollection queryString)
        {
            if (long.TryParse(queryString.Get("id"), out long deviceId))
            {
                var value = _deviceManager.GetValue(deviceId, out string message);

                return new ResponseDevice
                {
                    Success = true,
                    Content = message,
                    Value = value
                };
            }

            return Response.CreateFail("check parameter for device id");
        }

    }
}