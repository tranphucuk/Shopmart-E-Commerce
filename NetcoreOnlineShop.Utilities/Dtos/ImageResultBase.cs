using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NetcoreOnlineShop.Utilities.Dtos
{
    public abstract class ImageResultBase
    {
        public ImageResultBase()
        {
            ImgDate = DateTime.Now;
        }

        public DateTime ImgDate { get; set; }
    }
}
