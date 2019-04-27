using System.Linq;
using System.Net;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contract;

namespace SIS.HTTP.Responses
{
    public class HttpResult : IHttpResponse
    {
        private HttpStatusCode statusCode;

        public HttpResult(){ }


        public HttpResult(HttpResponseStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResult(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }
        public IHttpHeaderCollection Headers { get; private set; }
        public byte[] Content { get; set; }
        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode}")
                .AppendLine($"{this.Headers}")
                .AppendLine();

            return result.ToString();
        }
    }
}