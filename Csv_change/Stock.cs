using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvChange
{
    /// <summary>
    /// 定義整張字典用到的物件
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string DealDate { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 交易商ID
        /// </summary>
        public string SecBrokerID { get; set; }

        /// <summary>
        /// 交易商名稱
        /// </summary>
        public string SecBrokerName { get; set; }

        /// <summary>
        /// 交易價格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 買進量
        /// </summary>
        public int BuyQty { get; set; }

        /// <summary>
        /// 賣出量
        /// </summary>
        public int SellQty { get; set; }
    }
}
