using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;


namespace SignalRServer 
{
    public class RouteConnection : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            string msg = string.Format(
                "A new user [{0}] has just joined. (ID: {1})",
                request.QueryString["name"], connectionId);
            return Connection.Broadcast(msg);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            // Broadcast data to all clients
            string msg = string.Format(
                "{0}: {1}", request.QueryString["name"], data);
            return Connection.Broadcast(msg);
        }
    }
}