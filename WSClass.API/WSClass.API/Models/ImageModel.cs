using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSClass.API.Models
{
    public class ImageModel
    {
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public int Extension { get; set; }
        public FileModel File { get; set; }
        public ImageModel(Image image) {
            Width = image.Width;
            Height = image.Height;
            Extension = image.Extension;
            if (image.File1!=null)
            {
                File = new FileModel(image.File1);
            }
        }
    }
}