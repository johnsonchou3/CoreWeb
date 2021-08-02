using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 關括號的類別
    /// </summary>
    public class CloseBrac : Expression
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public CloseBrac(string value) : base(value)
        {
        }

        /// <summary>
        /// 關括號的權重
        /// </summary>
        /// <returns>權重為0</returns>
        public override int GetAssociativity()
        {
            return 0;
        }
    }
}
