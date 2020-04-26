using MiniMessengerServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MiniMessengerServer
{
    public class DataManager
    {
        private readonly IList<UserItem> _userItems = new List<UserItem>();
        private readonly CommonHelper _commonHelper = new CommonHelper();

        /// <summary>
        /// Key = toUserId, value = list of received message from all users.
        /// </summary>
        private readonly IDictionary<long, List<MessengerItem>> _messages = new Dictionary<long, List<MessengerItem>>();
        private readonly Timer _timer = new Timer();

        public DataManager()
        {
            this._userItems.Add(new UserItem { ID = this._commonHelper.CreateUserId(), IsOnline = true, Username = "Admin" });
            this._userItems.Add(new UserItem { ID = this._commonHelper.CreateUserId(), IsOnline = false, Username = "Test User" });

            this._messages.Add(this._userItems.First().ID,
                new[] {
                    new MessengerItem { ID = 1, Text = "Test Message", ToUserId = 2, UserId = 1, SendTime = DateTime.Now }
                }.ToList());


            this._timer.Interval = 5000;
            this._timer.Elapsed += this._timer_Elapsed;
            this._timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var item in this._userItems)
            {
                if(item.LastGet.AddSeconds(10) < DateTime.Now)
                {
                    item.IsOnline = false;
                }
            }
        }

        internal void UpdateUserAlive(long userId)
        {
            var userItem = this._userItems.FirstOrDefault(f => f.ID.Equals(userId));

            if(userItem == null)
            {
                return;
            }

            userItem.LastGet = DateTime.Now;
            userItem.IsOnline = true;
        }
        internal object GetUsers(long userId) => this._userItems.Where(w => w.ID != userId).ToArray();

        /// <summary>
        /// Return the messages for actual calling user.
        /// </summary>
        /// <param name="userId">Calling user id</param>
        /// <param name="toUserId">Get message from user</param>
        /// <returns></returns>
        internal object GetMessages(long userId, long toUserId)
        {
            // get all message for this user
            var allMessage = this._messages.Where(w => w.Key.Equals(userId)).SelectMany(s => s.Value);
            
            // get all message
            allMessage = allMessage.Where(w => w.UserId.Equals(toUserId));

            if (this.IsAdmin(toUserId) &&
                !allMessage.Any())
            {
                this.AddMessage(toUserId, userId, "Hallo ich bin der Admin Benutzer");

                return this.GetMessages(userId, toUserId);
            }

            return allMessage.ToArray();
        }

        private bool IsAdmin(long toUserId)
        {
            return this._userItems.First(f => f.Username.Equals("Admin")).ID.Equals(toUserId);
        }

        internal UserItem AddUser(string username)
        {
            var existUser = this._userItems.FirstOrDefault(a => a.Username.Equals(username));
            if (existUser != null)
            {
                return existUser;
            }

            var user = new UserItem { ID = this._commonHelper.CreateUserId(), IsOnline = true, Username = username };

            this._userItems.Add(user);

            return user;
        }

        /// <summary>
        /// Add a new message for the target user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="toUserId"></param>
        /// <param name="text"></param>
        /// <param name="fromme"></param>
        /// <returns></returns>
        internal object AddMessage(long userId, long toUserId, string text, bool fromme = false)
        {
            Console.WriteLine($"Receive Message: {text}");

            var messageItem = new MessengerItem
            {
                ID = this._commonHelper.CreateMessageId(),
                SendTime = DateTime.Now,
                Text = text,
                UserId = userId,
                ToUserId = toUserId,
                FromMe = fromme
            };

            if (!this._messages.ContainsKey(toUserId))
            {
                this._messages.Add(toUserId, new List<MessengerItem>());
            }

            this._messages[toUserId].Add(messageItem);

            if (this.IsAdmin(toUserId))
            {
                string str = "Noch kann ich nicht antworten";
                Console.WriteLine(str);
                this.AddMessage(toUserId, userId, str);
            }

            return messageItem;
        }

    }
}
