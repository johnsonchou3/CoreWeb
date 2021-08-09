using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 讓tempinputstring 乘上-1, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class PositiveNegative : TempManipulation
    {
        /// <summary>
        /// 讓tempinputstring 乘上-1
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>乘上-1 後的tempinpustring</returns>
        public override string TempFunction(string tempinputstring)
        {
            double reversed = double.Parse(tempinputstring) * (-1D);
            return reversed.ToString();
        }
    }
}
