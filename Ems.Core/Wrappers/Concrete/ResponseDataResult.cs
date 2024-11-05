using Ems.Core.Enums;
using Ems.Core.Wrappers.Concrete;
using Ems.Core.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Wrappers.Concrete;

public class ResponseDataResult<T> : ResponseResult, IResponseDataResult<T>
{
    public ResponseDataResult(ResponseType responseType) : base(responseType)
    {
    }

    public ICollection<ResponseValidationResult> ResponseValidationResults { get; set; } = new List<ResponseValidationResult>();
    public T Data { get; set; }

    public ResponseDataResult(ResponseType responseType, string message) : base(responseType, message)
    {
    }

    public ResponseDataResult(ResponseType responseType, T data) : base(responseType)
    {
        Data = data;
    }

    public ResponseDataResult(IList<ResponseValidationResult> validationResults) : base(ResponseType.ValidationError)
    {
        ResponseValidationResults = validationResults;
    }
    public ResponseDataResult(IList<ResponseValidationResult> validationResults, T data) : base(ResponseType.ValidationError)
    {
        Data = data;
        ResponseValidationResults = validationResults;
    }
}
