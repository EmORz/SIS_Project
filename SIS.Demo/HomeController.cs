﻿using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contract;
using SIS.WebServer.Result;

namespace SIS.Demo
{
    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello, World!</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        } 
    }
}