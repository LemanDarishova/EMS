using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Entity.Accounds;

public class RegisterStatus : BaseEntity
{
    public string Value { get; set; }
    public ICollection<UserDetail> UserDetails { get; set; }
}
