using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 把Tempinputstring歸零, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class ClearEntry : TempManipulation
    {
        /// <summary>
        /// 把Tempinputstring歸零
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>回傳0 字串</returns>
        public override string TempFunction(string tempinputstring)
        {
            return "0";
        }
    }
}
