using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 全部按鍵功能的abstract 類別
    /// </summary>
    public abstract class AllActions
    {
        /// <summary>
        /// 所有按鍵都有的action, 會被controller呼叫執行相應的工作
        /// </summary>
        /// <param name="Caldata">需傳入該idkey 的caldata 以作修改</param>
        public abstract void CalDataActions(CalData Caldata);
    }
}
