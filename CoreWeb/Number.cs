using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    public class Number : Expression
    {
        public Number(string value) : base(value)
        {
        }
        public override int GetAssociativity()
        {
            return -1;
        }
    }
}
