using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSClass.API.Consumer.Models;

namespace WSClass.API.Consumer.Exceptions
{
    [Serializable]
    public class BadRequestException : ApiException
    {
        public BadRequestException(BadRequest badRequest) : base(badRequest.RequestMessage, badRequest.ResponseMessage)
        {
        }
        public BadRequestException(BadRequest badRequest, string message) : base(badRequest.RequestMessage, badRequest.ResponseMessage, message) { }
        public BadRequestException(BadRequest badRequest, string message, Exception inner) : base(badRequest.RequestMessage, badRequest.ResponseMessage, message, inner) { }
        protected BadRequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}