using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMapping());
                cfg.AddProfile(new ViewModelToDomainMapping());
            });
        }
    }
}
