using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 所有運算元/數字的抽象類別
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// 各自的字串
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">各自的字串</param>
        public Expression(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 取得各自的權重
        /// </summary>
        /// <returns></returns>
        public abstract int GetAssociativity();
        public abstract void ExpAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString);

    }
}
