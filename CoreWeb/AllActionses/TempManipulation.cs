using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 修改tempinpustring 相關的抽象類別, 在執行各自tempfunction 後會更新tempinpustring 
    /// </summary>
    public abstract class TempManipulation : AllActions
    {
        /// <summary>
        /// 被繼承者需宣告其tempfunction: 對tempinputstring 的動作
        /// </summary>
        /// <param name="tempinputstring">caldata的tempinputstring</param>
        /// <returns></returns>
        public abstract string TempFunction(string tempinputstring);

        /// <summary>
        /// caldataction : 執行完tempfunction 後更新tempinputstring
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            Caldata.TempInputString = TempFunction(Caldata.TempInputString).ToString();
        }
    }
}
