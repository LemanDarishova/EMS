using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Entity.Estates;

public class Category : EditedBaseEntity
{
    public Category() => Estates = [];

    public  string CategoryName{ get; set; }
    public ICollection<Estate> Estates{ get; set; }
}
