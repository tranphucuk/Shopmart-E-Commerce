﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Product
{
    public class SizeViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }
    }
}
