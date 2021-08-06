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

        /// <summary>
        /// 數字在建樹時該有的行為, createtree 時會呼叫
        /// </summary>
        /// <param name="StackNodeTree">存放樹的Stack</param>
        /// <param name="StackNodeString">存放還不確定要拿來當children 還是parent 的node</param>
        public override void ExpAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            StackNodeTree.Push(t3);
        }
    }
}
