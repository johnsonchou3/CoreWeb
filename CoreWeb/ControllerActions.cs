using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 儲存所有計算機行為的類別, controller 拿到method 字串後會在這邊找, !!呼叫時大小寫必須一致
    /// </summary>
    public class ControllerActions
    {
        /// <summary>
        /// 準備作動作的計算機資料
        /// </summary>
        private CalData Caldata { get; set; }

        /// <summary>
        /// 要作行動的按鈕值
        /// </summary>
        private string Btn { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="caldata">準備作動作的計算機資料</param>
        /// <param name="btn">要作行動的按鈕值</param>
        public ControllerActions(CalData caldata, string btn)
        {
            this.Caldata = caldata;
            this.Btn = btn;
        }

        /// <summary>
        /// 數字鍵的Post request, 把按鍵值加到TempInputString最後
        /// </summary>
        public void NumPad()
        {
            Caldata.IsOperating = false;
            if (Caldata.IsAfterExecute == true)
            {
                Caldata.IsAfterExecute = false;
                Caldata.TempInputString = Btn;
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
                    if (Btn == "0" && Caldata.TempInputString == "0")
                    {
                        return;
                    }
                    Caldata.TempInputString += Btn;
                    if (Btn != "0")
                    {
                        // 以防 03 04 0005 等字出現
                        Caldata.TempInputString = double.Parse(Caldata.TempInputString).ToString();
                    }
                }
                catch (FormatException)
                {
                    //tempinputstring = null 會出現, 這時候讓tempinputstring = 數字鍵
                    Caldata.TempInputString = Btn;
                }
            }
        }

        /// <summary>
        /// Operator 的Post Request, 會把tempinput 及operator 放到stringofoperation 作儲存
        /// </summary>
        public void Operation()
        {
            if (Caldata.IsOperating)
            {
                Caldata.StringOfOperation = Caldata.StringOfOperation.Remove(Caldata.StringOfOperation.Length - 1, 1) + Btn;
            }
            else
            {
                Caldata.IsOperating = true;
                if (Caldata.IsAfterBracket)
                {
                    AddtoList();
                    Caldata.StringOfOperation += Btn;
                    Caldata.IsAfterBracket = false;
                }
                else
                {
                    Caldata.StringOfOperation += double.Parse(Caldata.TempInputString).ToString() + Btn;
                    Caldata.Expressionlist.Add(new Number(Caldata.TempInputString));
                    AddtoList();
                    ClearTemp();
                }
            }
            Caldata.StoretoDisplay();

            void AddtoList()
            {
                if (Btn == "*" || Btn == "/")
                {
                    Caldata.Expressionlist.Add(new MultDiv(Btn));
                }
                else if (Btn == "+" || Btn == "-")
                {
                    Caldata.Expressionlist.Add(new AddSub(Btn));
                }
            }

            // 寫入後把TempInputString清空作下一次儲存
            void ClearTemp()
            {
                Caldata.TempInputString = "0";
            }
        }

        /// <summary>
        /// Execute的Post Request, 把TempInputString打包, 並透過運算StringOfOperation, 結果存在TempInputString
        /// </summary>
        public void Execute()
        {
            if (!Caldata.IsAfterBracket)
            {
                Caldata.StringOfOperation += Caldata.TempInputString;
                Caldata.Expressionlist.Add(new Number(Caldata.TempInputString));
                Caldata.IsAfterBracket = false;
            }
            Node ExpTree = Node.CreateTree(Caldata.Expressionlist);
            Caldata.Preordstring = "Pre-Order: \n";
            Caldata.Inordstring = "In-Order: \n";
            Caldata.Postordstring = "Post-Order: \n";
            GetPreorder(ExpTree);
            GetInorder(ExpTree);
            GetPostorder(ExpTree);
            Caldata.TempInputString = GetResult(ExpTree).ToString();
            Caldata.DisplayOperation = Caldata.StringOfOperation;
            Caldata.StringOfOperation = string.Empty;
            Caldata.Expressionlist.Clear();
            Caldata.IsAfterBracket = false;
            Caldata.IsOperating = false;
            Caldata.IsAfterExecute = true;

            // 把運算式(string) 加到URL POST 給WebAPI 作運算並回傳結果(string)
            double GetResult(Node root)
            {
                if (root != null)
                {
                    if (root.Left == null && root.Right == null)
                    {
                        return double.Parse(root.Value);
                    }
                    double left_val = GetResult(root.Left);
                    double right_val = GetResult(root.Right);
                    Dictionary<string, double> OperationMap = new Dictionary<string, double>();
                    OperationMap.Add("+", left_val + right_val);
                    OperationMap.Add("-", left_val - right_val);
                    OperationMap.Add("*", left_val * right_val);
                    OperationMap.Add("/", left_val / right_val);
                    return OperationMap[root.Value];
                }
                return 0;
            }

            // 以前序編歷Tree, 並把value 加到Preordstring 以顯示
            void GetPreorder(Node root)
            {
                if (root != null)
                {
                    Caldata.Preordstring += root.Value + " ";
                    GetPreorder(root.Left);
                    GetPreorder(root.Right);
                }
            }

            // 以中序編歷Tree, 並把value 加到Preordstring 以顯示
            void GetInorder(Node root)
            {
                if (root != null)
                {
                    GetInorder(root.Left);
                    Caldata.Inordstring += root.Value + " ";
                    GetInorder(root.Right);
                }
            }

            // 以後序編歷Tree, 並把value 加到Preordstring 以顯示
            void GetPostorder(Node root)
            {
                if (root != null)
                {
                    GetPostorder(root.Left);
                    GetPostorder(root.Right);
                    Caldata.Postordstring += root.Value + " ";
                }
            }
        }

        /// <summary>
        /// 開根號的post request, 把TempInputString作開根號
        /// </summary>
        public void Root()
        {
            double tempnum = double.Parse(Caldata.TempInputString);
            Caldata.TempInputString = Math.Sqrt(tempnum).ToString();
        }

        /// <summary>
        /// ClearEntry 的Post Request, 清空TempInputString
        /// </summary>
        public void ClearEntry()
        {
            Caldata.TempInputString = "0";
        }

        /// <summary>
        /// ClearAll的Post Request, 清空tempinput及把之前的所有輸入移除
        /// </summary>
        public void ClearAll()
        {
            Caldata.IsAfterBracket = false;
            Caldata.IsAfterExecute = false;
            Caldata.TempInputString = "0";
            //Clear Current Datas
            Caldata.StringOfOperation = string.Empty;
            Caldata.Expressionlist.Clear();
            Caldata.StoretoDisplay();
        }

        /// <summary>
        /// Decimal 的Post Request, 會在Tempinputstring 加上.數, 不作decimal轉換
        /// </summary>
        public void Dec()
        {
            if (Caldata.TempInputString.Contains("."))
            {
            }
            else
            {
                Caldata.TempInputString += ".";
            }
        }

        /// <summary>
        /// 正負號的Post Request, 多加+/-在TempInputString前面
        /// </summary>
        public void PosNeg()
        {
            string txtboxstr = Caldata.TempInputString;
            double reversed = double.Parse(txtboxstr) * (-1);
            Caldata.TempInputString = reversed.ToString();
        }

        /// <summary>
        /// Backspace的Post Request, 將TempInputString 最後char 刪除
        /// </summary>
        public void Backspace()
        {
            try
            {
                //Remove Last Digit
                string curtextbox = Caldata.TempInputString;
                Caldata.TempInputString = (curtextbox).Remove(Math.Max(1, curtextbox.Length - 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                Caldata.TempInputString = "0";
            }
        }

        /// <summary>
        /// 開括號的Post Request, 把"(" 存進Expressionlist 及Stringofoperation, 並更新exeoper
        /// </summary>
        public void BracketOp()
        {
            Caldata.Expressionlist.Add(new OpBrac(Btn));
            Caldata.StringOfOperation += Btn;
            Caldata.BracketOpCount += 1;
            Caldata.StoretoDisplay();
        }

        /// <summary>
        /// 關括號的Post request, 加入關括號, 把目前的輸入字串存起來, 並把IsAfterBracket 改為True 讓Operation 及 Execute 作判斷以免出錯
        /// </summary>
        public void BracketClose()
        {
            if (Caldata.BracketOpCount > Caldata.BracketCloseCount)
            {
                Caldata.StringOfOperation += Caldata.TempInputString;
                Caldata.Expressionlist.Add(new Number(Caldata.TempInputString));
                Caldata.Expressionlist.Add(new CloseBrac(Btn));
                Caldata.StringOfOperation += Btn;
                Caldata.BracketCloseCount += 1;
                Caldata.StoretoDisplay();
                Caldata.TempInputString = "0";
                Caldata.IsAfterBracket = true;
            }
        }

        /// <summary>
        /// 倒數的Post request, 把tempinputstring 倒過來, 再取代Tempinputstring
        /// </summary>
        public void Invert()
        {
            double InvertedDouble = 1 / double.Parse(Caldata.TempInputString);
            Caldata.TempInputString = InvertedDouble.ToString();
        }
    }
}
