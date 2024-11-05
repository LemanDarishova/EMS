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

public class CategoryConfig : EditedBaseEntityConfig<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CategoryName)
            .HasColumnName("Category Name")
            .HasMaxLength(30);

        builder.HasData([
            new Category {Id=1,CategoryName="Yeni tikili",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Category {Id=2,CategoryName="Köhnə tikili",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Category {Id=3,CategoryName="Həyət evi",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Category {Id=4,CategoryName="Ofis",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            new Category {Id=5,CategoryName="Obyekt",CreatedId=1,CreatedDate=DateTime.UtcNow,UpdatedId=1,UpdatedDate=DateTime.UtcNow},
            ]);
    }
}
