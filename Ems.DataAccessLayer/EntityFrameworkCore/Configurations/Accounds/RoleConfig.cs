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

public class RoleConfig : EditedBaseEntityConfig<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.RoleName)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("Role");

        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);


        builder.HasData([
            new Role {Id=1,RoleName="Admin",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Role {Id=2,RoleName="Agentlik",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Role {Id=3,RoleName="Ev Sahibi",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow}
        ]);
    }
}
