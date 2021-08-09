using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 把url的method 做成相應的allactions(button), 製作前要先把Button 設成url的button
    /// </summary>
    public static class AllActionsFactory
    {
        /// <summary>
        /// 透過傳入的method 及button 回傳相應的allaction 物件
        /// </summary>
        /// <param name="method">url 中的method </param>
        /// <param name="Button">url 中的button </param>
        /// <returns></returns>
        public static AllActions CreateObject(string method, string Button)
        {
            Dictionary<string, AllActions> ExpressionFactory = new Dictionary<string, AllActions>()
                {
                    {"numpad", new Number(Button)},
                    {"multiplydivision", new MultiplyDivision(Button)},
                    {"addsubtract", new AddSubtract(Button)}, 
                    {"execute", new Execute()}, 
                    {"bracketclose", new BracketClose(Button)}, 
                    {"bracketopen", new BracketOpen(Button)}, 
                    {"root", new Root()}, 
                    {"invert", new Invert()}, 
                    {"positivenegative", new PositiveNegative()}, 
                    {"clearentry", new ClearEntry()}, 
                    {"clearall", new ClearAll()}, 
                    {"backspace", new Backspace()}, 
                    {"adddecimal", new AddDecimal()}, 
                };
            return ExpressionFactory[method];
        }
    }
}
