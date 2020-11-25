using WSClass.API.Consumer.Enums;

namespace WSClass.API.Consumer.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int Leaght { get; set; }
    }
}