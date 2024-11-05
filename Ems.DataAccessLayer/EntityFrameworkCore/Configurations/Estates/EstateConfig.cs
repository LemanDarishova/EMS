using Ems.DataAccessLayer.EntityFrameworkCore.Configurations.Commans;
using Ems.Entity.Estates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Configurations.Estates;

public class EstateConfig : EditedBaseEntityConfig<Estate> 
{
    public override void Configure(EntityTypeBuilder<Estate> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Title");

        builder.Property(x => x.Area)
            .IsRequired()
            .HasColumnName("Area");

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("Address");

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnName("Price");

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Estates)
            .HasForeignKey(x => x.CategoryId);

        builder.HasOne(x => x.Users)
            .WithMany(x => x.Estates)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.UploadedFiles)
            .WithOne()
            .HasForeignKey(x => x.EstateId);
    }


}
