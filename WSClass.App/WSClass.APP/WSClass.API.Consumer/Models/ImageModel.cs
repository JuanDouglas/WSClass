using System;

namespace WSClass.API.Consumer.Models
{
    public class ImageModel
    {
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public int Extension { get; set; }
        public FileModel File { get; set; }
    }
}