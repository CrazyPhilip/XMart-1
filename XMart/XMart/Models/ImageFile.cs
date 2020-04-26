using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XMart.Models
{
    public class ImageFile
    {
        public string PreviewPath { get; set; }
        public string Path { get; set; }
        public Stream ImgStream { get; set; }
    }
}
