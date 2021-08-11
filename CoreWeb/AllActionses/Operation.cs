using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 加減乘除的抽象類別, 繼承expression 類別, 
    /// </summary>
    public abstract class Operation : Expression
    {
        /// <summary>
        /// 建構子: 需傳入operator 本身的字串 +-*/
        /// </summary>
        /// <param name="value">operator 本身的字串 +-*/</param>
        public Operation(string value) : base(value)
        {
        }

        /// <summary>
        /// 在建樹時的動作, 在遇到把自己權重小的node 時才會存入樹中, (:-2, 數字: -1, ): 0,  +-: 1, */: 2
        /// </summary>
        /// <param name="StackNodeTree">存入tree 的Stack</param>
        /// <param name="StackNodeString">暫存node 的Stack</param>
        public override void CreateTreeAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            while (StackNodeString.Peek().Associativity != -2 && StackNodeString.Peek().Associativity >= this.GetAssociativity())
            {
                Node t = StackNodeString.Pop();
                Node t1 = StackNodeTree.Pop();
                Node t2 = StackNodeTree.Pop();
                t.Left = t2;
                t.Right = t1;
                StackNodeTree.Push(t);
            }
            StackNodeString.Push(t3);
        }

        /// <summary>
        /// Operator 的Post Request, 會把tempinput 及operator 放到stringofoperation 作儲存
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            //如果在運算中就刷新運算子
            if (Caldata.IsOperating)
            {
                Caldata.StringOfOperation = Caldata.StringOfOperation.Remove(Caldata.StringOfOperation.Length - 1, 1) + Value;
            }
            else
            {
                Caldata.IsOperating = true;
                //如果在括號之後就不要加tempinput
                if (Caldata.IsAfterBracket)
                {
                    AddtoList();
                    Caldata.StringOfOperation += Value;
                    Caldata.IsAfterBracket = false;
                }
                else
                {
                    Caldata.StringOfOperation += double.Parse(Caldata.TempInputString).ToString() + Value;
                    Caldata.Expressionlist.Add(new Number(Caldata.TempInputString));
                    AddtoList();
                    ClearTemp();
                }
            }
            Caldata.StoretoDisplay();

            void AddtoList()
            {
                if (Value == "*" || Value == "/")
                {
                    Caldata.Expressionlist.Add(new MultiplyDivision(Value));
                }
                else if (Value == "+" || Value == "-")
                {
                    Caldata.Expressionlist.Add(new AddSubtract(Value));
                }
            }

            // 寫入後把TempInputString清空作下一次儲存
            void ClearTemp()
            {
                Caldata.TempInputString = "0";
            }
        }
    }
}
