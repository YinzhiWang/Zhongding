using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Extension
{
    public static class DecimalExtension
    {
        public static bool BiggerThanZero(this decimal? value)
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
