using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb
{
    /// <summary>
    /// 每個用家的計算機數據, 在每個request 都會回傳給用家
    /// </summary>
    public class CalData
    {
        /// <summary>
        /// 判斷目前是否在AfterBracket, 以免operation或execute 出現格式錯誤
        /// </summary>
        public bool IsAfterBracket { get; set; } = false;

        /// <summary>
        /// 判斷目前是否在operation, 在operation當中再按operation 會修改operator
        /// </summary>
        public bool IsOperating { get; set; } = false;

        /// <summary>
        /// 判斷是否剛進行了等於, 以便在運算後重置數字
        /// </summary>
        public bool IsAfterExecute { get; set; } = false;

        /// <summary>
        /// 紀錄目前開括號數量
        /// </summary>
        public int BracketOpCount { get; set; } = 0;

        /// <summary>
        /// 紀錄目前關括號數量
        /// </summary>
        public int BracketCloseCount { get; set; } = 0;

        /// <summary>
        /// 目前的運算式, 每按operation/bracket/execute 都會使其更新, 在execute/ClearAll後會清空
        /// </summary>
        public string StringOfOperation { get; set; }

        /// <summary>
        /// 讓form1 讀取顯示, 透過storedisplay存取stringofoperation, 唯execute後不清空以便用家看到式子
        /// </summary>
        public string DisplayOperation { get; set; }

        /// <summary>
        /// 讓form1 讀取顯示, 在execute後會顯示expressiontree 的前序
        /// </summary>
        public string Preordstring { get; set; }

        /// <summary>
        /// 讓form1 讀取顯示, 在execute後會顯示expressiontree 的中序
        /// </summary>
        public string Inordstring { get; set; }

        /// <summary>
        /// 讓form1 讀取顯示, 在execute後會顯示expressiontree 的後序
        /// </summary>
        public string Postordstring { get; set; }

        /// <summary>
        /// 目前輸入的String, 會顯示在form1textbox 讓用家知道目前輸入
        /// </summary>
        public string TempInputString { get; set; } = "0";

        /// <summary>
        /// 存入每個operand 及operator 的List, 以便在execute 創建tree
        /// </summary>
        public List<Expression> Expressionlist { get; set; } = new List<Expression>();

        /// <summary>
        /// 內建的儲存displayoperation method 方便使用
        /// </summary>
        public void StoretoDisplay()
        {
            DisplayOperation = StringOfOperation;
        }
    }
}
