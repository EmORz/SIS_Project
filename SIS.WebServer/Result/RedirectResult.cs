using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Result
{
    public class RedirectResult : HttpResult
    {
        public RedirectResult(string location) :
            base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.Add(new HttpHeader("location", location));
            
        }
        
    }
}