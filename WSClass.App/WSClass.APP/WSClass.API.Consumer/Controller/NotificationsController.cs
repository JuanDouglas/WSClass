using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WSClass.API.Consumer;
using WSClass.API.Consumer.Enums;
using WSClass.API.Consumer.Exceptions;
using WSClass.API.Consumer.Models;

namespace WSClass.API.Consumer.Controllers
{
    public class NotificationsController
    {
        /// <summary>
        /// Obtém as notificações do usuário.
        /// </summary>
        /// <param name="login_token">Token de Autenticação</param>
        /// <param name="valid_key">Chave de validação da autenticação.</param>
        public async System.Threading.Tasks.Task<Notification[]> GetNotificationsAsync(string login_token, string valid_key) {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();

            UriBuilder requestUri = new UriBuilder(General.APIUri);
            requestUri.Path = "/api/Notification";
            requestUri = new UriBuilder(QueryHelpers.AddQueryString(requestUri.ToString(),
                new Dictionary<string, string>() { { "login_token", login_token }, { "valid_key", valid_key } }));

            requestMessage.RequestUri = requestUri.Uri;
            requestMessage.Method = HttpMethod.Get;

            var response = await client.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
               NotificationAPI[] notifications = JsonConvert.DeserializeObject<NotificationAPI[]>(await response.Content.ReadAsStringAsync());
                List<Notification> returnNotifications = new List<Notification>();
                foreach (var item in notifications)
                {
                    if (item.Icon!=null)
                    {
                        FilesController filesController = new FilesController();
                        byte[] file = await filesController.DownloadAsync(item.Icon.File.FileName,
                            "JPG",
                            FileType.ImageFile);
                        MemoryStream stream = new MemoryStream(file);

                        returnNotifications.Add(new Notification(item.ID, item.Title, item.Content, System.Drawing.Image.FromStream(stream)));
                    }

                    returnNotifications.Add(new Notification(item.ID,item.Title,item.Content));
                }

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