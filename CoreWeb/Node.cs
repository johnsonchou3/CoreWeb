using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb
{
    /// <summary>
    /// 樹節點的class
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 節點本身的值(operand/operator)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 節點值的權重
        /// </summary>
        public int Associativity { get; set; }

        /// <summary>
        /// 左邊Children的節點
        /// </summary>
        public Node Left { get; set; }

        /// <summary>
        /// 右邊Children的節點
        /// </summary>
        public Node Right { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">節點本身的值(operand/operator)</param>
        public Node(Expression c)
        {
            Value = c.value;
            Left = Right = null;
            Associativity = c.GetAssociativity();
        }

        /// <summary>
        /// 以expression list 創建Tree, 在一開始先以stack儲存數字及+-*/, 並在list 前後加上"()"以判斷式子是否結束
        /// 在遇到下一個operator 時, 透過dictionary 判斷要先以前者或後者作節點
        /// 最後再回傳Tree的root, 也是StackNode的最上層(Peek)
        /// </summary>
        /// <param name="Expressionlist">需要Btn的ExpressionList, iterate 每一個operand/operator</param>
        /// <returns></returns>
        public static Node CreateTree(List<string> Expressionlist)
        {
            Stack<Node> StackNodeTree = new Stack<Node>();
            Stack<Node> StackNodeString = new Stack<Node>();
            Node t;
            Node t1;
            Node t2;
            Node t3;
            Expressionlist.Insert(0, "(");
            Expressionlist.Add(")");
            foreach (string oper in Expressionlist)
            {
                if (oper == "*" || oper == "/")
                {
                    MultDiv exp = new MultDiv(oper);
                    t3 = new Node(exp);
                }
                else if (oper == "+" || oper == "-")
                {
                    AddSub exp = new AddSub(oper);
                    t3 = new Node(exp);
                }
                else if (oper == ")")
                {
                    CloseBrac exp = new CloseBrac(oper);
                    t3 = new Node(exp);
                }
                else
                {
                    Number exp = new Number(oper);
                    t3 = new Node(exp);
                }
                if (oper == "(")
                {
                    StackNodeString.Push(t3);
                }
                else if (t3.Associativity == -1)
                {
                    StackNodeTree.Push(t3);
                }
                else if (t3.Associativity > 0)
                {
                    while (StackNodeString.Peek().Value != "(" && StackNodeString.Peek().Associativity >= t3.Associativity)
                    {
                        t = StackNodeString.Pop();
                        t1 = StackNodeTree.Pop();
                        t2 = StackNodeTree.Pop();
                        t.Left = t2;
                        t.Right = t1;
                        StackNodeTree.Push(t);
                    }
                    StackNodeString.Push(t3);
                }
                else if (oper == ")")
                {
                    while (StackNodeString.Peek().Value != "(")
                    {
                        //前面的再把它弄成樹(括號內)
                        t = StackNodeString.Peek();
                        StackNodeString.Pop();
                        t1 = StackNodeTree.Peek();
                        StackNodeTree.Pop();
                        t2 = StackNodeTree.Peek();
                        StackNodeTree.Pop();
                        t.Left = t2;
                        t.Right = t1;
                        StackNodeTree.Push(t);
                    }
                    StackNodeString.Pop();
                }
            }
            t = StackNodeTree.Peek();
            return t;
        }
    }
}
