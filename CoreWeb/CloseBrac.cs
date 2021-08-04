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
        public override void ExpAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            while (StackNodeString.Peek().Value != "(")
            {
                //前面的再把它弄成樹(括號內)
                Node t = StackNodeString.Peek();
                StackNodeString.Pop();
                Node t1 = StackNodeTree.Peek();
                StackNodeTree.Pop();
                Node t2 = StackNodeTree.Peek();
                StackNodeTree.Pop();
                t.Left = t2;
                t.Right = t1;
                StackNodeTree.Push(t);
            }
            StackNodeString.Pop();
        }
    }
}
