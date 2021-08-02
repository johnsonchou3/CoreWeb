using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 數字的類別
    /// </summary>
    public class Number : Expression
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public Number(string value) : base(value)
        {
        }

        /// <summary>
        /// 取得權重
        /// </summary>
        /// <returns>權重為-1</returns>
        public override int GetAssociativity()
        {
            return -1;
        }
    }
}
