using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Extensions;

public static class ByteExtensions
{
    public static string ByteToString(this byte[] byteItem)
    {
        return Convert.ToBase64String(byteItem);
    }

    public static byte[] ToByte(this string stringItem)
    {
        return Convert.FromBase64String(stringItem);
    }
}
