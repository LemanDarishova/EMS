using Ems.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Wrappers.Interfaces;

public interface IResponseResult
{
    public string Message {  get; set; }
    ResponseType ResponseType { get; set; }
}
