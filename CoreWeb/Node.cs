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
            Value = c.Value;
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
        public static Node CreateTree(List<Expression> Expressionlist)
        {
            Stack<Node> StackNodeTree = new Stack<Node>();
            Stack<Node> StackNodeString = new Stack<Node>();
            Node t;
            Expressionlist.Insert(0, new OpBrac("("));
            Expressionlist.Add(new CloseBrac(")"));
            foreach (Expression exp in Expressionlist)
            {
                exp.ExpAction(StackNodeTree, StackNodeString);
            }
            t = StackNodeTree.Peek();
            return t;
        }
    }
}
