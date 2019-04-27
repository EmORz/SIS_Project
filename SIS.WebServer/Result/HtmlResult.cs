using System.Net;
using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Result
{
    public class HtmlResult : HttpResult
    {
        private string content;
        private HttpResponseStatusCode ok;

        public HtmlResult(string content, HttpStatusCode statusCode) : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public HtmlResult(string content, HttpResponseStatusCode ok)
        {
            this.content = content;
            this.ok = ok;
        }
    }
}