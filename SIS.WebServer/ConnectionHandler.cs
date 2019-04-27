using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
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
        
        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesRead==0)
                {
                    break;
                }
                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.AppendLine(bytesAsString);
                if (numberOfBytesRead<1023)
                {
                    break;
                }

                if (result.Length==0)
                {
                    return null;
                }
            }
            var tempHttpRequest = new HttpRequest(result.ToString());
            return tempHttpRequest;
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {

            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)||!this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResult(HttpResponseStatusCode.NotFound);
            }
            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            ArraySegment<byte> byteSegments = new ArraySegment<byte>(Encoding.UTF8.GetBytes(httpResponse.ToString()));
         
            await client.SendAsync(byteSegments, SocketFlags.None);
            
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();
            if (httpRequest!=null)
            {
                var httpResponse = this.HandleRequest(httpRequest);
                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

       
    }
}