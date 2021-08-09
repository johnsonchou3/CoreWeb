using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 把tempinpustring 變為其倒數, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class Invert : TempManipulation
    {
        /// <summary>
        /// 把tempinpustring 變為其倒數
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>tempinputstring 的倒數</returns>
        public override string TempFunction(string tempinputstring)
        {
            double Inverted = 1D / double.Parse(tempinputstring);
            return Inverted.ToString();
        }
    }
}
