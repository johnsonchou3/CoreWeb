using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    public class MultDiv : Expression
    {
        public MultDiv(string value) : base(value)
        {
        }
        public override int GetAssociativity()
        {
            return 2;
        }
    }
}
