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
        public override void CreateTreeAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            StackNodeTree.Push(t3);
        }

        /// <summary>
        /// 數字鍵的Post request, 把按鍵值加到TempInputString最後
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
            Caldata.IsOperating = false;
            if (Caldata.IsAfterExecute == true)
            {
                Caldata.IsAfterExecute = false;
                Caldata.TempInputString = Value;
                Caldata.StoretoDisplay();
            }
            else
            {
                AddNum();
                Caldata.StoretoDisplay();
            }

            // 把按鍵值加到TempInputString最後
            void AddNum()
            {
                try
                {
                    if (Value == "0" && Caldata.TempInputString == "0")
                    {
                        return;
                    }
                    Caldata.TempInputString += Value;
                    if (Value != "0")
                    {
                        // 以防 03 04 0005 等字出現
                        Caldata.TempInputString = double.Parse(Caldata.TempInputString).ToString();
                    }
                }
                catch (FormatException)
                {
                    //tempinputstring = null 會出現, 這時候讓tempinputstring = 數字鍵
                    Caldata.TempInputString = Value;
                }
            }
            Caldata.IsAfterBracket = false;
        }
    }
}
