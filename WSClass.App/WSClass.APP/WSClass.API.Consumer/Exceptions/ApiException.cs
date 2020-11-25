using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace WSClass.API.Consumer.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; set; }
        public HttpRequestMessage RequestMessage { get; set; }
        public ApiException(HttpRequestMessage request, HttpResponseMessage response)
        {
            ResponseMessage = response;
            RequestMessage = request;
        }
        public ApiException(HttpRequestMessage request, HttpResponseMessage response, string message) : base(message)
        {
            ResponseMessage = response;
            RequestMessage = request;
        }
        public ApiException(HttpRequestMessage request, HttpResponseMessage response, string message, Exception inner) : base(message, inner)
        {
            ResponseMessage = response;
            RequestMessage = request;
        }
        protected ApiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}