using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSClass.API.Models.Enums;

namespace WSClass.API.Models
{
    public class UploadSession
    {
        public Guid Token { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ValidToken { get; set; }
        public string Post { get; set; }
        public FileType FileType { get; set; }

        public UploadSession(Guid token, DateTime createDate, Guid validToken, string post, FileType fileType)
        {
            Token = token;
            CreateDate = createDate;
            ValidToken = validToken;
            Post = post ?? throw new ArgumentNullException(nameof(post));
            FileType = fileType;
        }
    }
}