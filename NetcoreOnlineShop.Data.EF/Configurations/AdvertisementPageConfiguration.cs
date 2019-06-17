using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreOnlineShop.Data.EF.Extensions;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Configurations
{
    public class AdvertisementPageConfiguration : DbEntityConfiguration<AdvertisementPage>
    {
        public override void Configure(EntityTypeBuilder<AdvertisementPage> entity)
        {
            entity.Property(ap => ap.Id).HasMaxLength(20).IsRequired();
        }
    }
}
