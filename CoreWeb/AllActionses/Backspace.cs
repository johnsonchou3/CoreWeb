using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 後退鍵的功能, 繼承tempmanipulation 的calaction, 執行tempfunction後會更新tempinputstring
    /// </summary>
    public class Backspace : TempManipulation
    {
        /// <summary>
        /// 刪掉tempinpustring 最後一個char, 如果已經沒東西可刪就會回傳0
        /// </summary>
        /// <param name="tempinputstring">原本的tempinputstring</param>
        /// <returns>倒退後的tempinpustring</returns>
        public override string TempFunction(string tempinputstring)
        {
            try
            {
                //Remove Last Digit
                string curtextbox = tempinputstring;
                return (curtextbox).Remove(Math.Max(1, curtextbox.Length - 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                return "0";
            }
        }
    }
}
