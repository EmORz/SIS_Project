using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contract;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        
        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            this.client = client;
            this.serverRoutingTable = serverRoutingTable;

        }

        //TODO Implement this method and complete project!!
        private async Task<IHttpRequest> ReadRequest()
        {
            return null;
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            return null;
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            return;
        }

        public  async  Task ProcessRequestAsync() { }

       
    }
}