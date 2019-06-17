using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class FooterConfiguration : DbEntityConfiguration<Footer>
    {
        public override void Configure(EntityTypeBuilder<Footer> entity)
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasMaxLength(256).IsRequired();
        }
    }
}
