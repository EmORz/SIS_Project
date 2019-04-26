using System.Net;
using System.Text;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Result
{
    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpStatusCode statusCode) : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type","text/plain"));
            this.Content = Encoding.UTF8.GetBytes(content);

        }
        
    }
}