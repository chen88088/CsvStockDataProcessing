using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvChange
{
    /// <summary>
    /// 第三個按鈕所需要的物件
    /// </summary>
    public class StockBuySellOver
    {
        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }
        
        /// <summary>
        /// 經銷商ID 
        /// </summary>
        public string SecBrokerID { get; set; }
        
        /// <summary>
        /// 經銷商名稱 
        /// </summary>
        public string SecBrokerName { get; set; }
        
        /// <summary>
        /// 交易量差 
        /// </summary>
        public int BuySellOver { get; set; }

        /// <summary>
        /// 進入列表的優先次序
        /// </summary>
        public int SortedIndex { get; set; }
    }
}
