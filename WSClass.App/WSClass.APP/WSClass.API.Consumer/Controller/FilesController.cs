using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WSClass.API.Consumer.Enums;
using WSClass.API.Consumer.Exceptions;
using WSClass.API.Consumer.Models;

namespace WSClass.API.Consumer.Controllers
{
    public class FilesController
    {
        public async Task<byte[]> DownloadAsync(string filename, string imageExtension,FileType fileType) {
            HttpClient client = new HttpClient();

            UriBuilder requestUri = new UriBuilder(General.APIUri);
            requestUri.Path = "/api/Download";
            requestUri = new UriBuilder(QueryHelpers.AddQueryString(requestUri.ToString(),
                new Dictionary<string, string>() {
                                { "filename", filename},
                                { "ImageExtension", imageExtension},
                                { "filetype", fileType.ToString()}
                }));

            HttpResponseMessage response = await client.GetAsync(requestUri.ToString());
            if (response.StatusCode==HttpStatusCode.OK)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(new BadRequest(response,
                    null,
                    JsonConvert.DeserializeObject<State>(await response.Content.ReadAsStringAsync())));
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new InternalServeErrorException(null, response);
            }

            throw new ApiException(null, response);
        }
    }
}
