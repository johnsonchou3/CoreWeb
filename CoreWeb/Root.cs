using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 讓tempinpustring 開根號, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class Root : TempManipulation
    {
        /// <summary>
        /// 讓tempinpustring 開根號
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>開根號後的tempinpustring</returns>
        public override string TempFunction(string tempinputstring)
        {
            double rooted = Math.Sqrt(double.Parse(tempinputstring));
            return rooted.ToString();
        }
    }
}
