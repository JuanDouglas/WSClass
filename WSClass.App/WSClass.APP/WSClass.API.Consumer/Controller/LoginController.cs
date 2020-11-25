using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using WSClass.API.Consumer.Models;
using WSClass.API.Consumer;
using WSClass.API.Consumer.Exceptions;
using Microsoft.AspNetCore.WebUtilities;

namespace WSClass.API.Consumer.Controllers
{
    public class LoginController
    {

        public async Task<Login> CreateAsync(Login login)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(login));

            UriBuilder requestUri = new UriBuilder(General.APIUri);
            requestUri.Path = "/api/Login/Authetication/Create";
            requestMessage.RequestUri = requestUri.Uri;
            requestMessage.Method = HttpMethod.Put;

            var response = await client.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Login>(await response.Content.ReadAsStringAsync());
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(new BadRequest(response,
                    requestMessage,
                    JsonConvert.DeserializeObject<State>(await response.Content.ReadAsStringAsync())));
            }
            throw new NotImplementedException();
        }
        public async Task<Authenticated> LoginAsync(string user, string password)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();

            UriBuilder requestUri = new UriBuilder(General.APIUri);
            requestUri.Path = "/api/Login/Authetication";
            requestUri = new UriBuilder(QueryHelpers.AddQueryString(requestUri.ToString(),
                new Dictionary<string, string>() { { "user", user }, { "pas", password } }));

            requestMessage.RequestUri = requestUri.Uri;
            requestMessage.Method = HttpMethod.Get;

            var response = await client.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Authenticated>(await response.Content.ReadAsStringAsync());
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(new BadRequest(response,
                    requestMessage,
                    JsonConvert.DeserializeObject<State>(await response.Content.ReadAsStringAsync())));
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new InternalServeErrorException(requestMessage, response);
            }
            throw new ApiException(requestMessage, response);
        }
    }
}