using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Abstract;
public interface ICategoryService
{
    public Task<IDictionary<int, string>> GetCategoriesDictionaryAsync();
}
