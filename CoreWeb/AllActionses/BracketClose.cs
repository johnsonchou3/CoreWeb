using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 關括號的類別
    /// </summary>
    public class BracketClose : Brackets
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="value">本身的字串</param>
        public BracketClose(string value = ")") : base(value)
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

        /// <summary>
        /// 關括號在建樹時該有的行為, createtree 時會呼叫
        /// </summary>
        /// <param name="StackNodeTree">存放樹的Stack</param>
        /// <param name="StackNodeString">存放還不確定要拿來當children 還是parent 的node</param>
        public override void CreateTreeAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            while (StackNodeString.Peek().Associativity != -2)
            {
                //除非遇到權重-2 ["("], 不然通通做成樹
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

        /// <summary>
        /// 關括號的Post request, 加入關括號, 把目前的輸入字串存起來, 並把IsAfterBracket 改為True 讓Operation 及 Execute 作判斷以免出錯
        /// </summary>
        /// <param name="Caldata">Idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            if (Caldata.BracketOpCount > Caldata.BracketCloseCount)
            {
                Caldata.BracketCloseCount += 1;
                Caldata.StringOfOperation += Caldata.TempInputString;
                Caldata.Expressionlist.Add(new Number(Caldata.TempInputString));
                base.CalDataActions(Caldata);
                Caldata.TempInputString = "0";
                Caldata.IsAfterBracket = true;
            }
        }
    }
}
