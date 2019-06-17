using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class BlogTagConfiguration : DbEntityConfiguration<BlogTag>
    {
        public override void Configure(EntityTypeBuilder<BlogTag> entity)
        {
            entity.Property(bt => bt.TagId).HasMaxLength(255).IsRequired().HasColumnType("varchar(255)");
        }
    }
}
