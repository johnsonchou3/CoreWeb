using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    public abstract class Expression
    {
        public string value { get; set; }
        public Expression(string value)
        {
            this.value = value;
        }
        public abstract int GetAssociativity();
    }
}
