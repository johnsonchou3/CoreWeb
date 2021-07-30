using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// + 或- 的類別, 
    /// </summary>
    public class AddSub : Expression
    {
        public AddSub(string value) : base(value)
        {
        }
        public override int GetAssociativity()
        {
            return 1;
        }
    }
}
