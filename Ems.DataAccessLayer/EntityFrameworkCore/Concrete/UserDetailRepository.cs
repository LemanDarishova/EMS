using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.Entity.Accounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;

public class UserDetailRepository : GenericRepository<UserDetail>, IUserDetailRepository
{
    public UserDetailRepository(EmsContext emsContext) : base(emsContext)
    {
    }
}
