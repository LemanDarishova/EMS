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

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(EmsContext emsContext) : base(emsContext)
    {
    }

    public async Task<IDictionary<int, string>> GetCategoryDictionaryAsync()
    {
        return await EmsEntity.ToDictionaryAsync(x => x.Id, x => x.CategoryName);
    }
}
