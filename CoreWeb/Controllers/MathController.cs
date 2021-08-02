//MathController.cs file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Threading;
using System.Collections.Concurrent;

namespace CoreWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]

    /// <summary>
    /// 計算機的controller
    /// </summary>
    public class MathController : Controller
    {
        /// <summary>
        /// 計算機的cache, 會有所有用家的caldata
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// lock 物件, 確保每次只有一個request 傳入以免有race condition
        /// </summary>
        private static object Lock = new object ();

        /// <summary>
        /// 建構子, 每次使用都會存取memorycache
        /// </summary>
        /// <param name="_memoryCache">系統的memorycache</param>
        public MathController(IMemoryCache _memoryCache)
        {
            this.cache = _memoryCache;
        }

        /// <summary>
        /// 當用家用get時會跳出提示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "This is a calculator WebAPI, please use Post Method to call this API";
        }

        /// <summary>
        /// 數字鍵的Post request, 把按鍵值加到TempInputString最後
        /// </summary>
        /// <param name="btnnum">數字鍵本身的值</param>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Numpad")]
        public IActionResult Numpad(string btnnum)
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                var a = cache.Get(Idkey);
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.IsOperating = false;
                AddNum();
                caldata.StoretoDisplay();
                return Ok(caldata);

                // 把按鍵值加到TempInputString最後
                void AddNum()
                {
                    try
                    {
                        caldata.TempInputString += btnnum;
                        caldata.TempInputString = double.Parse(caldata.TempInputString).ToString();
                    }
                    catch (FormatException)
                    {
                        //tempinputstring = null 會出現, 這時候讓tempinputstring = 數字鍵
                        caldata.TempInputString = btnnum;
                    }
                }
            }
        }

        /// <summary>
        /// Operator 的Post Request, 會把tempinput 及operator 放到stringofoperation 作儲存
        /// </summary>
        /// <param name="btnop">使用鍵本身的值</param>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Operation")]
        public IActionResult Operation(string btnop)
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                if (caldata.IsOperating)
                {
                    caldata.StringOfOperation = caldata.StringOfOperation.Remove(caldata.StringOfOperation.Length - 1, 1) + btnop;
                }
                else
                {
                    caldata.IsOperating = true;
                    if (caldata.IsAfterBracket)
                    {
                        caldata.Expressionlist.Add(btnop);
                        caldata.StringOfOperation += btnop;
                        caldata.IsAfterBracket = false;
                    }
                    else
                    {
                        SaveValue();
                        ClearTemp();
                    }
                }
                caldata.StoretoDisplay();
                return Ok(caldata);

                // 把TempInputString及目前的operator寫入StringOfOperation中
                void SaveValue()
                {
                    caldata.StringOfOperation += double.Parse(caldata.TempInputString).ToString() + btnop;
                    caldata.Expressionlist.Add(caldata.TempInputString);
                    caldata.Expressionlist.Add(btnop);
                }

                // 寫入後把TempInputString清空作下一次儲存
                void ClearTemp()
                {
                    caldata.TempInputString = "0";
                }
            }
        }

        /// <summary>
        /// Execute的Post Request, 把TempInputString打包, 並透過運算StringOfOperation, 結果存在TempInputString
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Execute")]
        public IActionResult Execute()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                if (!caldata.IsAfterBracket)
                {
                    SaveValue();
                    caldata.IsAfterBracket = false;
                }
                try
                {
                    Node ExpTree = Node.CreateTree(caldata.Expressionlist);
                    caldata.Preordstring = "Pre-Order: \n";
                    caldata.Inordstring = "In-Order: \n";
                    caldata.Postordstring = "Post-Order: \n";
                    GetPreorder(ExpTree);
                    GetInorder(ExpTree);
                    GetPostorder(ExpTree);
                    caldata.TempInputString = GetResult(ExpTree).ToString();
                    caldata.DisplayOperation = caldata.StringOfOperation;
                    caldata.StringOfOperation = string.Empty;
                    caldata.Expressionlist.Clear();
                    caldata.IsAfterBracket = false;
                    return Ok(caldata);
                }
                catch
                {
                    return BadRequest("Input Error, please clear all and try again");
                }

                // 把目前輸入值加進運算式中
                void SaveValue()
                {
                    caldata.StringOfOperation += caldata.TempInputString;
                    caldata.Expressionlist.Add(caldata.TempInputString);
                }

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
                        caldata.Preordstring += root.Value + " ";
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
                        caldata.Inordstring += root.Value + " ";
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
                        caldata.Postordstring += root.Value + " ";
                    }
                }
            }
        }

        /// <summary>
        /// 開根號的post request, 把TempInputString作開根號
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Root")]
        public IActionResult Root()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                double tempnum = double.Parse(caldata.TempInputString);
                caldata.TempInputString = Math.Sqrt(tempnum).ToString();
                return Ok(caldata);
            }
        }

        /// <summary>
        /// ClearEntry 的Post Request, 清空TempInputString
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("ClearEntry")]
        public IActionResult ClearEntry()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.TempInputString = "0";
                return Ok(caldata);
            }
        }

        /// <summary>
        /// ClearAll的Post Request, 清空tempinput及把之前的所有輸入移除
        /// </summary>
        /// <returns></returns>
        [HttpPost("ClearAll")]
        public IActionResult ClearAll()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.IsAfterBracket = false;
                caldata.TempInputString = "0";
                ClearDatas();
                caldata.StoretoDisplay();
                return Ok(caldata);

                void ClearDatas()
                {
                    caldata.StringOfOperation = string.Empty;
                    caldata.Expressionlist.Clear();
                }
            }
        }

        /// <summary>
        /// Decimal 的Post Request, 會在Tempinputstring 加上.數, 不作decimal轉換
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Dec")]
        public IActionResult Dec()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.TempInputString += ".";
                return Ok(caldata);
            }
        }

        /// <summary>
        /// 正負號的Post Request, 多加+/-在TempInputString前面
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("PosNeg")]
        public IActionResult PosNeg()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                SwitchPosNeg();
                return Ok(caldata);

                void SwitchPosNeg()
                {
                    string txtboxstr = caldata.TempInputString;
                    decimal reversed = decimal.Parse(txtboxstr) * (-1);
                    caldata.TempInputString = reversed.ToString();
                }
            }
        }

        /// <summary>
        /// Backspace的Post Request, 將TempInputString 最後char 刪除
        /// </summary>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("Backspace")]
        public IActionResult Backspace()
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                try
                {
                    RemoveLastDigit();
                }
                catch (ArgumentOutOfRangeException)
                {
                    caldata.TempInputString = "0";
                }
                return Ok(caldata);

                void RemoveLastDigit()
                {
                    string curtextbox = caldata.TempInputString;
                    caldata.TempInputString = (curtextbox).Remove(Math.Max(1, curtextbox.Length - 1));
                }
            }
        }

        /// <summary>
        /// 開括號的Post Request, 把"(" 存進Expressionlist 及Stringofoperation, 並更新exeoper
        /// </summary>
        /// <param name="bracket">使用按鍵本身括號值</param>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("BracketOp")]
        public IActionResult BracketOp(string bracket)
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.Expressionlist.Add(bracket);
                caldata.StringOfOperation += bracket;
                caldata.StoretoDisplay();
                return Ok(caldata);
            }
        }

        /// <summary>
        /// 關括號的Post request, 加入關括號, 把目前的輸入字串存起來, 並把IsAfterBracket 改為True 讓Operation 及 Execute 作判斷以免出錯
        /// </summary>
        /// <param name="bracket">使用按鍵本身括號值</param>
        /// <returns>回傳新的caldata</returns>
        [HttpPost("BracketClose")]
        public IActionResult BracketClose(string bracket)
        {
                string Idkey = Request.Cookies["ID"];
                if (Idkey == null)
                {
                    Idkey = Guid.NewGuid().ToString();
                }
                Response.Cookies.Append("ID", Idkey);
            lock (Lock)
            {
                CalData caldata;
                caldata = cache.GetOrCreate(Idkey, entry => new CalData());
                caldata.StringOfOperation += caldata.TempInputString;
                caldata.Expressionlist.Add(caldata.TempInputString);
                caldata.Expressionlist.Add(bracket);
                caldata.StringOfOperation += bracket;
                caldata.StoretoDisplay();
                caldata.TempInputString = "0";
                caldata.IsAfterBracket = true;
                return Ok(caldata);
            }
        }
    }
}