using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class SystemConfiguration : DbEntityConfiguration<SystemConfig>
    {
        public override void Configure(EntityTypeBuilder<SystemConfig> entity)
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasMaxLength(255).IsRequired();
        }
    }
}
