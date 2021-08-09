using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 建樹時用到的開括號的類別
    /// </summary>
    public class BracketOpen : Brackets
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">開括號的值(字串)</param>
        public BracketOpen(string value = "(") : base(value)
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
        public override void CreateTreeAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            StackNodeString.Push(t3);
        }

        /// <summary>
        /// 開括號的Post Request, 把"(" 存進Expressionlist 及Stringofoperation, 並更新exeoper
        /// </summary>
        /// <param name="Caldata">相應idkey 的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            Caldata.BracketOpCount += 1;
            base.CalDataActions(Caldata);
        }
    }
}
