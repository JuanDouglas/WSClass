using WSClass.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using System.Web;
using WSClass.API.Models.Enums;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Routing;
using System.Web.SessionState;
using WSClass.API.Models.Exceptions;
using System.Web.Http.Description;

namespace WSClass.API.Controllers
{
    [RoutePrefix("api/Files")]
    public class FilesController : ApiController
    {
        private TaskDatabaseEntities db = new TaskDatabaseEntities();
        private static List<UploadSession> Sessions { get; set; }

        private static List<UploadModel> Uploads { get; set; }
        //[HttpGet]
        //[Route("Image/GetFile")]
        //// GET: api/Files
        //public async Task<IHttpActionResult> Get(string filename)
        //{
        //    var imageModel = await db.Image.FirstOrDefaultAsync(fs=>fs.Name==filename);
        //    HttpResponseMessage message;
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(
        //        new Uri(imageModel.FileName));
        //    request.Method = WebRequestMethods.Ftp.DownloadFile;
        //    request.Credentials = new NetworkCredential(Settings.Default.ftpUser,
        //        Settings.Default.ftpPas);
        //    request.UseBinary = true;
        //    var response = request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    MemoryStream reader =  new MemoryStream();
        //    await stream.CopyToAsync(reader);
        //    byte[] content = reader.ToArray();
        //    message = new HttpResponseMessage(HttpStatusCode.OK) {
        //        Content = new ByteArrayContent(content)
        //    };
        //    string contentType =$"image/{imageModel.Name.Split('.')[imageModel.Name.Split('.').Length - 1]}";
        //    message.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        //    return ResponseMessage(message);
        //}
        [HttpGet]
        [ResponseType(typeof(System.Drawing.Image))]
        [Route("Download")]
        // GET: api/Files
        public async Task<IHttpActionResult> Get(string filename, FileType filetype, ImageExtension? extension)
        {
            if (filetype==FileType.ImageFile)
            {
                Models.Image imgModel = await db.Image.FirstOrDefaultAsync(fs => fs.File1.Path == filename);
                if (imgModel == null)
                {
                    return NotFound();
                }

                string basePath = GetPartialDirectory(filetype);
                string path = $"{basePath}{filename}";

                if (!System.IO.File.Exists(path))
                {
                    return NotFound();
                }
                FileStream flStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                System.Drawing.Image img = System.Drawing.Image.FromStream(flStream);

                MemoryStream ms = new MemoryStream();
                img.Save(ms, GetFormat(extension.Value));
                HttpResponseMessage message = new HttpResponseMessage
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                message.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{extension}");
                return ResponseMessage(message);
            }
            if (true)
            {

            }
            return NotFound();
        }

        // POST: api/Files
        [HttpPost()]
        [Route("Upload/Image/{upload_session}/{valid_Key}")]
        public async Task<IHttpActionResult> Post(Guid upload_session, Guid valid_Key)
        {
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;

                UploadSession session = GetUploadSession(upload_session);
                Models.Image img = null;

                if (session == null)
                {
                    return NotFound();
                }

                if (session.ValidToken != valid_Key)
                {
                    return Unauthorized();
                }

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = $"{session.FileType}_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff")}.jpeg";

                    var path = GetPartialDirectory(FileType.ImageFile);

                    System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);

                    img = new Models.Image()
                    {
                        Width = image.Width,
                        Height = image.Height,
                    };

                    FileStream fs = new FileStream(path + $"\\{fileName}",
                        FileMode.OpenOrCreate,
                        FileAccess.Write);

                    try
                    {
                        db.Image.Add(img);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw new APIException(e.Message, e, null, null);
                    }

                    img = await db.Image.FirstOrDefaultAsync(first => first.File1.Path == fileName);

                    image.Save(fs, ImageFormat.Jpeg);
                }
                bool exist = true;
                Guid validToken;
                do
                {
                    validToken = Guid.NewGuid();
                    if (Uploads == null)
                    {
                        Uploads = new List<UploadModel>();
                    }
                    bool vavatoken = Uploads.FirstOrDefault(fs => fs.VerifyToken == validToken) == null;

                    if (vavatoken)
                    {
                        exist = false;
                    }
                } while (exist);

                UploadModel model = new UploadModel(session.Token, validToken, img.ID, DateTime.Now);
                Uploads.Add(model);
                session.Post = session.Post.Replace("%7bupload_session%7d", upload_session.ToString());
                session.Post = session.Post.Replace("%7bimage_id%7d", img.ID.ToString());
                session.Post = session.Post.Replace("%7bverify_token%7d", model.VerifyToken.ToString());


                return Redirect(session.Post);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Files/5
        public void Put(int id, [FromBody] string value)
        {


        }

        // DELETE: api/Files/5
        public void Delete(int id)
        {
        }
        public string GetPartialDirectory(FileType type)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            switch (type)
            {
                case FileType.ImageFile:
                    return baseDirectory + "\\Files\\Images\\";
                case FileType.CompactFile:
                    return baseDirectory + "\\Files\\Compacts\\";
                default:
                    return baseDirectory;
            }
        }
        public ImageFormat GetFormat(ImageExtension extension)
        {
            switch (extension)
            {
                case ImageExtension.jpeg:
                    return ImageFormat.Jpeg;
                case ImageExtension.bmp:
                    return ImageFormat.Bmp;
                case ImageExtension.png:
                    return ImageFormat.Png;
                case ImageExtension.icon:
                    return ImageFormat.Icon;
                case ImageExtension.gif:
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        public UploadSession GetUploadSession(Guid token)
        {
            return Sessions.FirstOrDefault(fs => fs.Token == token);
        }
        public static UploadSession GenerateUploadSession(string url_edit, FileType fileType)
        {
            if (Sessions == null)
            {
                Sessions = new List<UploadSession>();
            }
            bool exist = true;
            Guid token;
            Guid validToken;
            do
            {

                token = Guid.NewGuid();
                validToken = Guid.NewGuid();

                bool vatoken = Sessions.FirstOrDefault(fs => fs.Token == token) == null;
                bool vavatoken = Sessions.FirstOrDefault(fs => fs.ValidToken == validToken) == null;
                if (vatoken && vavatoken)
                {
                    exist = false;
                }
            } while (exist);
            UploadSession session = new UploadSession(token, DateTime.Now, validToken, url_edit, fileType);
            Sessions.Add(session);
            return session;
        }
        private void RemoveSession(UploadSession session)
        {
            Sessions.RemoveAll(ps => ps.Token == session.Token);
        }
        public static bool ValidUpload(Guid upload_session, Guid valid_key, int image_id)
        {
            var upload = Uploads.FirstOrDefault(fs => fs.UploadSession == upload_session);
            if (upload == null)
            {
                return false;
            }
            if (!upload.VerifyToken.ToString().Equals(valid_key.ToString()))
            {
                return false;
            }
            if (upload.Image_ID != image_id)
            {
                return false;
            }
            Uploads.RemoveAll(re => re.UploadSession == upload_session);
            return true;
        }
    }
}