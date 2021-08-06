using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 建樹時用到的開括號的類別
    /// </summary>
    public class OpBrac : Expression
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">開括號的值(字串)</param>
        public OpBrac(string value) : base(value)
        {
        }

        /// <summary>
        /// 開括號的權重
        /// </summary>
        /// <returns>權重為0</returns>
        public override int GetAssociativity()
        {
            return -2;
        }

        /// <summary>
        /// 開括號在建樹時該有的行為, createtree 時會呼叫
        /// </summary>
        /// <param name="StackNodeTree">存放樹的Stack</param>
        /// <param name="StackNodeString">存放還不確定要拿來當children 還是parent 的node</param>
        public override void ExpAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            StackNodeString.Push(t3);
        }
    }
}
