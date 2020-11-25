using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSClass.API.Models
{
    public class UploadModel
    {
        public Guid UploadSession { get; set; }
        public Guid VerifyToken { get; set; }
        public int Image_ID { get; set; }
        public DateTime CreateDate { get; set; }

        public UploadModel(Guid uploadSession, Guid verifyToken, int image_ID, DateTime createDate)
        {
            UploadSession = uploadSession;
            VerifyToken = verifyToken;
            Image_ID = image_ID;
            CreateDate = createDate;
        }
    }
}