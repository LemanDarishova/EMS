using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Helpers;

public static class CodeGenerator
{
    private static readonly Random _random = new Random();

    public static int GenerateConfirmCode(int minLength = 6, int maxLength = 6)
    {
        if (minLength < 1 || maxLength < minLength)
        {
            throw new ArgumentException("Invalid length parameters.");
        }

        int min = (int)Math.Pow(10, minLength - 1);
        int max = (int)Math.Pow(10, maxLength) - 1;

        return _random.Next(min, max + 1);
    }
}
