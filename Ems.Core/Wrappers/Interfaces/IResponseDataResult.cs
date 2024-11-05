using Ems.Core.Wrappers.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Wrappers.Interfaces;

public interface IResponseDataResult<T> : IResponseResult
{
    T Data { get; set; }
    ICollection<ResponseValidationResult> ResponseValidationResults { get; set; }
}
