using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChatV2.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> userList = new Dictionary<string, string>();

        public async Task SendMessage(string user, string message, string sendTo)
        {
            try
            {
                userList.Add(Context.ConnectionId,user);
            }
            catch 
            {

            }

            string sendId = userList.FirstOrDefault(u => u.Value == sendTo).Key;
            if (sendId != null)
            {
                await Clients.Client(sendId).SendAsync("ReceiveMessage", user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            

            string uList = BuildUserString ();
            await Clients.All.SendAsync("ReceiveList",uList);
        }

        public async Task ListUsers()
        {
            string uList = BuildUserString ();
            await Clients.All.SendAsync("ReceiveList",uList);
        }

        public async Task ClosePage(string user)
        {
            try
            {
                userList.Remove(Context.ConnectionId);

            }
            catch
            {

            }
            await Clients.All.SendAsync("ReceiveMessage", user, "Goodbye");

            string uList = BuildUserString ();
            await Clients.All.SendAsync("ReceiveList", uList);

        }

        private string BuildUserString ()
        {
            string uList = "<ul>";
            foreach( var item in userList.Values)
            {
                uList = uList + "<li>" + item + "</li>";
            }
            uList = uList + "</ul>";
            return uList;
        }


    }
}