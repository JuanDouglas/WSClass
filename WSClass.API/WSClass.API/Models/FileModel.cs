using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSClass.API.Models.Enums;

namespace WSClass.API.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int Leaght { get; set; }
        public FileModel(File file) {
            FileName = file.Path;
            FileType = (FileType)file.FileType;
            Leaght = file.Leaght;
        }
    }
}