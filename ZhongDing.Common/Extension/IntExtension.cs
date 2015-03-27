using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Extension
{
    public static class IntExtension
    {
        public static bool BiggerThanZero(this int? value)
        {
            if (value.HasValue)
            {
                if (value.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
