using Ems.Entity.Accounds;
using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Ems.Core.Enums;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly EmsContext _emsContext;

    public UserRepository(EmsContext emsContext) : base(emsContext)
    {
        _emsContext = emsContext;
    }


    public Task<User> GetSigninUserWithDetailAsync(string email)
    {
        return _emsContext.Users
            .Include(x => x.UserDetail)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

}
