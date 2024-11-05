using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.Entity.Estates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;

public class EstateRepository : GenericRepository<Estate>, IEstateRepository
{
    public EstateRepository(EmsContext emsContext) : base(emsContext)
    {
    }


    public async Task<IEnumerable<Estate>> GetEstateWithDetailsAsync()
    {
        return await EmsEntity
             .Include(x => x.Category)
             .Include(x => x.UploadedFiles)
             .OrderByDescending(x => x.Id)
             .AsNoTrackingWithIdentityResolution()
             .ToListAsync();

    }
}