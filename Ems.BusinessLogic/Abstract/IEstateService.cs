using Ems.BusinessLogic.Dtos;
using Ems.Core.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Abstract;
public interface IEstateService
{
    Task<IResponseDataResult<bool>> AddAsync(AddEstateDto estateDto);
    Task<IResponseDataResult<IEnumerable<GetEstateViewDto>>> GetAllAsync();
    Task<IResponseDataResult<bool>> RemoveAsync(int id);
    Task<IResponseDataResult<bool>> UpdateAsync(int id);
}
