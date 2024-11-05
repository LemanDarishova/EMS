using Ems.Entity.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.Abstract;

public interface IEstateRepository : IGenericRepository<Estate>
{
    Task<IEnumerable<Estate>> GetEstateWithDetailsAsync();
}
