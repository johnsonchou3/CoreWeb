using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// clearall 的類別, 繼承allactions類別
    /// </summary>
    public class ClearAll : AllActions
    {
        /// <summary>
        /// ClearAll的Post Request, 清空tempinput及把之前的所有輸入移除
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            Caldata.IsAfterBracket = false;
            Caldata.IsAfterExecute = false;
            Caldata.BracketOpCount = 0;
            Caldata.BracketCloseCount = 0;
            Caldata.TempInputString = "0";
            //Clear Current Datas
            Caldata.StringOfOperation = string.Empty;
            Caldata.Expressionlist.Clear();
            Caldata.StoretoDisplay();
        }
    }
}
