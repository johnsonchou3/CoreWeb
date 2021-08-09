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
using System.Reflection;

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
        private static object Lock = new object();

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
        /// Handle 所有post request的method, 會把url 裡的method btn 丟到controlleraction 裡轉換成行動再執行
        /// </summary>
        /// <param name="method">url 裡的method 參數</param>
        /// <param name="button">url 裡的btn 參數, 代表按鈕本身的值</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RouteActions([FromQuery] string method, string button)
        {
            if (method == null)
            {
                return BadRequest("Method name cannot be null!");
            }
            method = method.ToLower();

            //Dictionary<string, AllActions> ExpressionFactory = new Dictionary<string, AllActions>()
            //    {
            //        {"numpad" , new Number(btn)},
            //        {"multiplydivision" , new MultiplyDivision(btn)},
            //        {"addsubtract" , new AddSubtract(btn)},
            //        {"execute" , new Execute()},
            //        {"bracketclose" , new BracketClose(btn)},
            //        {"bracketopen" , new BracketOpen(btn)},
            //        {"root" , new Root()},
            //        {"invert" , new Invert()},
            //        {"positivenegative" , new PositiveNegative()},
            //        {"clearentry" , new ClearEntry()},
            //        {"clearall" , new ClearAll()},
            //        {"backspace" , new Backspace()},
            //        {"adddecimal" , new AddDecimal()},

            //    };
            ////method 沒東西的時候

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
                    AllActions currentaction = AllActionsFactory.CreateObject(method, button);
                    currentaction.CalDataActions(caldata);
                    return Ok(caldata);
                }
                catch (KeyNotFoundException)
                {
                    return BadRequest("You probably messed up the method name");
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
            }
        }
    }
}