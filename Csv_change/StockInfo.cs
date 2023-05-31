namespace CsvChange
{
    /// <summary>
    /// 第二顆按鈕字典需要的物件
    /// </summary>
    public class StockInfo
    {
        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 買進總量
        /// </summary>
        public int BuyTotal { get; set; }

        /// <summary>
        /// 賣出總量
        /// </summary>
        public int SellTotal { get; set; }

        /// <summary>
        /// 平均價格
        /// </summary>
        public double AvgPrice { get; set; }

        /// <summary>
        /// 交易量差
        /// </summary>
        public int BuySellOver { get; set; }

        /// <summary>
        /// 經銷商數量
        /// </summary>
        public int SecBrokerCnt { get; set; }
    }
}