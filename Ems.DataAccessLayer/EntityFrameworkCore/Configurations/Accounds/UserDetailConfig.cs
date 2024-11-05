using Ems.DataAccessLayer.EntityFrameworkCore.Configurations.Commans;
using Ems.Entity.Accounds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Configurations.Accounds;

public class UserDetailConfig : BaseEntityConfig<UserDetail>
{
    public override void Configure(EntityTypeBuilder<UserDetail> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("FirstName");

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("LastName");

        builder.Ignore(x => x.FullName);

        builder.Property(x => x.DateOfBirth)
            .IsRequired(false)
            .HasColumnType("datetime")
            .HasColumnName("DateOfBirth");

        builder.Property(x => x.ConfirmCode)
            .IsRequired(false)
            .HasColumnName("ConfirmCode");

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}
