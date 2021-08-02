using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 乘除的類別
    /// </summary>
    public class MultDiv : Expression
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public MultDiv(string value) : base(value)
        {
        }

        /// <summary>
        /// 取得權重
        /// </summary>
        /// <returns>權重為2</returns>
        public override int GetAssociativity()
        {
            return 2;
        }
    }
}
