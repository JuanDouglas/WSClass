using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace WSClass.API.Consumer.Exceptions
{
    [Serializable]
    public class InternalServeErrorException : ApiException
    {
        public InternalServeErrorException(HttpRequestMessage request, HttpResponseMessage response) : base(request, response)
        {

        }
        public InternalServeErrorException(HttpRequestMessage request, HttpResponseMessage response, string message) : base(request, response, message) { }
        public InternalServeErrorException(HttpRequestMessage request, HttpResponseMessage response, string message, Exception inner) : base(request, response, message, inner) { }
        protected InternalServeErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}