using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 開關括號的抽象類別, 都有類似的caldataactions
    /// </summary>
    public abstract class Brackets : Expression
    {
        /// <summary>
        /// 建構子: 需要本身的括號的字串"("")"
        /// </summary>
        /// <param name="value">本身的括號的字串"("")"</param>
        public Brackets(string value) : base(value)
        {
        }

        /// <summary>
        /// 開關括號的Post Request, 把"(" 存進Expressionlist 及Stringofoperation, 並更新exeoper
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            Caldata.Expressionlist.Add(new BracketOpen(Value));
            Caldata.StringOfOperation += Value;
            Caldata.StoretoDisplay();
        }
    }
}
