using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestStringPath)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestStringPath);

        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException();
            }

            this.Url = requestLine[1];
        }

        private void ParseRequest(string requestStringPath)
        {
            //Todo make buisness logic
            var splitRequestContent = requestStringPath.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = splitRequestContent[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.ValidateRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
           // if (splitRequestContent.Length > 1)
            //{
            var requestHasBody = splitRequestContent.Length > 1;
                this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
            //}
        }

        private void ParseRequestParameters(string bodyParameters,  bool requestHasBody)
        {
            this.ParseQueryParameters(this.Url);
            if (requestHasBody)
            {
                this.ParseFromDataParameters(bodyParameters);
            }
        }

        private void ParseQueryParameters(string url)
        {
            var queryParameters = this.Url?.Split(new []{'?', '#'}).Skip(1).ToArray()[0];

            if (string.IsNullOrEmpty(queryParameters))
            {
                throw new BadRequestException();
            }

            var queryKeyValuePairs = queryParameters.Split(new []{'&'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryKeyValuePair in queryKeyValuePairs)
            {
                var keyValuePair = queryKeyValuePair.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                if (keyValuePair.Length!=2)
                {
                    throw new BadRequestException();
                }
                var queryKey = keyValuePair[0];
                var queryValue = keyValuePair[1];

                this.QueryData[queryKey]= queryValue;
            }
        }

        private void ParseFromDataParameters(string bodyParameters)
        {
       

            var formKeyValuePairs = bodyParameters.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var formKeyValuePair in formKeyValuePairs)
            {
                var keyValuePair = formKeyValuePair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }
                var formDataKey = keyValuePair[0];
                var formDataValue = keyValuePair[1];
                this.QueryData[formDataKey]= formDataValue;
            }
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }

            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeaders = requestHeader.Split(
                   new[] { ':', ' ' }
                    ,
                    StringSplitOptions.RemoveEmptyEntries);
                var requestHeaderKey = splitRequestHeaders[0];
                var requestHeaderValue = splitRequestHeaders[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));

            }
        }

        private void ParseRequestPath()
        {
            var path = this.Url?.Split('?').FirstOrDefault();
            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }

            var parseResult = Enum.TryParse<HttpRequestMethod>(requestLine[0], out var parsRequestMethod);

            if (!parseResult)
            {
                throw new BadRequestException();
            }
            this.RequestMethod = parsRequestMethod;
        }

        private bool ValidateRequestLine(string[] requestLine)
        {
            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }
            return false;
        }



        public string Path { get; private set; }
        public string Url { get; private set; }
        public Dictionary<string, object> FormData { get; }
        public Dictionary<string, object> QueryData { get; }
        public IHttpHeaderCollection Headers { get; }
        public HttpRequestMethod RequestMethod { get; private set; }
    }
}