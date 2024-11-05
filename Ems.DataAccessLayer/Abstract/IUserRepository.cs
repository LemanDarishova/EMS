using Ems.Entity.Accounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.Abstract;

public interface IUserRepository :IGenericRepository<User>
{
    Task<User> GetSigninUserWithDetailAsync(string email);

}
