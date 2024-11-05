using Ems.Core.Enums;
using Ems.Core.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Wrappers.Concrete;

public class ResponseResult : IResponseResult
{
    public ResponseResult(ResponseType responseType) => ResponseType = responseType;

    public ResponseResult(ResponseType responseType,string message)
    {
        ResponseType = responseType;
        Message = message;
    }
    public string Message { get; set; }
    public ResponseType ResponseType { get; set; }
}
