using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 在tempuinputstring 增加小數點的功能, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class AddDecimal : TempManipulation
    {
        /// <summary>
        /// 在tempinpustring 後加上"."
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>加上點數後的tempinpustring</returns>
        public override string TempFunction(string tempinputstring)
        {
            if (tempinputstring.Contains("."))
            {
                return tempinputstring;
            }
            else
            {
                return tempinputstring += ".";
            }
        }
    }
}
