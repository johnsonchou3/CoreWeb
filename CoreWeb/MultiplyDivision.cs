using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 乘除的類別
    /// </summary>
    public class MultiplyDivision : Operation
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public MultiplyDivision(string value) : base(value)
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

        /// <summary>
        /// 乘或除在建樹時該有的行為, createtree 時會呼叫
        /// </summary>
        /// <param name="StackNodeTree">存放樹的Stack</param>
        /// <param name="StackNodeString">存放還不確定要拿來當children 還是parent 的node</param>
        //public void CreateTreeAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        //{
        //    Node t3 = new Node(this);
        //    while (StackNodeString.Peek().Associativity != -2 && StackNodeString.Peek().Associativity >= this.GetAssociativity())
        //    {
        //        Node t = StackNodeString.Pop();
        //        Node t1 = StackNodeTree.Pop();
        //        Node t2 = StackNodeTree.Pop();
        //        t.Left = t2;
        //        t.Right = t1;
        //        StackNodeTree.Push(t);
        //    }
        //    StackNodeString.Push(t3);
        //}
        //*****************繼承 Operation 的
    }
}
