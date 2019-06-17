using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class LanguageConfiguration : DbEntityConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.Property(lan => lan.Id).HasMaxLength(50).IsRequired();
        }
    }
}
