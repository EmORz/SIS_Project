using System.Net;
using System.Text;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Result
{
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpStatusCode statusCode) : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

    }
}