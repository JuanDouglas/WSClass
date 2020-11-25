using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace WSClass.API.Consumer.Models
{
    public class BadRequest
    {
        public HttpResponseMessage ResponseMessage { get; set; }
        public HttpRequestMessage RequestMessage { get; set; }
        public State ModelState { get; set; }

        public BadRequest(HttpResponseMessage responseMessage, HttpRequestMessage requestMessage, State modelState)
        {
            ResponseMessage = responseMessage ?? throw new ArgumentNullException(nameof(responseMessage));
            RequestMessage = requestMessage ?? throw new ArgumentNullException(nameof(requestMessage));
            ModelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
        }
    }
}