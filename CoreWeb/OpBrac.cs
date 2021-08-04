using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    public class OpBrac : Expression
    {
        public OpBrac(string value) : base(value)
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
        public override void ExpAction(Stack<Node> StackNodeTree, Stack<Node> StackNodeString)
        {
            Node t3 = new Node(this);
            StackNodeString.Push(t3);
        }

    }
}
