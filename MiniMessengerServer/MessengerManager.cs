using MiniMessengerServer.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MiniMessengerServer
{
    public class MessengerManager
    {
        private readonly IDictionary<string, Func<NameValueCollection, Response>> _methods = new Dictionary<string, Func<NameValueCollection, Response>>();

        public MessengerManager()
        {
            this._methods.Add("/getOnlineUser", CommonHelper.GetOnlineUser);
            this._methods.Add("/getMessages", CommonHelper.GetMessages);
            this._methods.Add("/addUser", CommonHelper.AddUser);
            this._methods.Add("/sendMessage", CommonHelper.SendMessage);

            this._methods.Add("/deviceGetAll", CommonHelper.GetAll);
            this._methods.Add("/deviceSendCommand", CommonHelper.DeviceSendCommand);
            this._methods.Add("/deviceGetValue", CommonHelper.DeviceGetValue);
        }

        /// <summary>
        /// Load the file content.
        /// </summary>
        /// <param name="method">method name</param>
        /// <param name="funcGetData">for data response.</param>
        /// <returns>Return the page or data in string.</returns>
        public string GetData(string method, NameValueCollection queryString)
        {
            if (!this._methods.ContainsKey(method))
            {
                SimpleLogging.Log($"unknow method: {method}");
                return "Missing method!";
            }

            var response = this._methods[method].Invoke(queryString);

            return JsonConvert.SerializeObject(response);
        }
    }
}
