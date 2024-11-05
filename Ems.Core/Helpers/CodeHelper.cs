using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Helpers;

public class CodeHelper
{
    public static int GenerateConfirmCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999); 
    }
}
