using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Dtos
{
    public class ImageResult : ImageResultBase
    {
        public string FileSize { get; set; }

        public string ImgValueBase64 { get; set; }

        public string FileName { get; set; }

        public string ImgPath { get; set; }

        public string Url { get; set; }
    }
}
