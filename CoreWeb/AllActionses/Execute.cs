using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// Execute的類別, 繼承allactions
    /// </summary>
    public class Execute : AllActions
    {
        /// <summary>
        /// Execute的Post Request, 把TempInputString打包, 並透過運算StringOfOperation, 結果存在TempInputString
        /// </summary>
        /// <param name="Caldata">idkey 相應的caldata</param>
        public override void CalDataActions(CalData Caldata)
        {
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
                Caldata.BracketOpCount = 0;
                Caldata.BracketCloseCount = 0;

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
        }
    }
}
