using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// + 或- 的類別, 
    /// </summary>
    public class AddSub : Expression
    {
        /// <summary>
        /// 加減號的建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public AddSub(string value) : base(value)
        {
        }

        /// <summary>
        /// 加減號的權重
        /// </summary>
        /// <returns>權重為1</returns>
        public override int GetAssociativity()
        {
            return 1;
        }
    }
}
