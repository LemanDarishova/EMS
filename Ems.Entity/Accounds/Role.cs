﻿using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Entity.Accounds;

public class Role : EditedBaseEntity
{
    public Role() => UserRoles = [];
    public string RoleName{ get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
