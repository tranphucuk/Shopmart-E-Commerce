using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasMaxLength(128).IsRequired().HasColumnType("varchar(128)");
        }
    }
}
