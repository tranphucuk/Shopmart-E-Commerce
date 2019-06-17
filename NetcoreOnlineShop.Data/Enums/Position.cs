using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Data.Enums
{
    public enum Position
    {
        [Description("Top center")]
        TopCenter,
        [Description("Top left")]
        TopLeft,
        [Description("Top right")]
        TopRight,
        [Description("Middle Center")]
        MiddleCenter,
        [Description("Middle left")]
        MiddleLeft,
        [Description("Middle right")]
        MiddleRight,
        [Description("Bottom center")]
        BottomCenter,
        [Description("Bottom left")]
        BottomLeft,
        [Description("Bottom right")]
        BottomRight,
    }
}
