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

public class RegisterStatusConfig : BaseEntityConfig<RegisterStatus>
{
    public override void Configure(EntityTypeBuilder<RegisterStatus> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Value)
            .HasMaxLength(30)
            .HasColumnName("Value");
    }
}
