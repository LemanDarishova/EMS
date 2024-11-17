using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.Entity.Accounds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly EmsContext _emsContext;
    public RoleRepository(EmsContext emsContext) : base(emsContext)
    {
        _emsContext = emsContext;
    }

    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        return await _emsContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
    }
}
