using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Reflection;

namespace CsvChange
{
    /// <summary>
    /// 主框架
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 啟動
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 計時
        /// </summary>
        private Stopwatch StopWatch = new Stopwatch();

        /// <summary>
        /// 傳出textbox文字的變數
        /// </summary>
        private string AllText;

        /// <summary>
        /// comboboset所用的hash set
        /// </summary>
        private static HashSet<string> comboUseHashSet = new HashSet<string>();

        /// <summary>
        /// 紀錄所有stock id的hashset
        /// </summary>
        private static HashSet<string> dictKeysHashSet = new HashSet<string>();

        /// <summary>
        /// 總字典
        /// </summary>
        private static Dictionary<string, List<Stock>> DgvReadFileDict = new Dictionary<string, List<Stock>>();

        /// <summary>
        /// 第二按鈕用的字典
        /// </summary>
        private Dictionary<string, List<StockInfo>> DgvSearchDict = new Dictionary<string, List<StockInfo>>();

        /// <summary>
        /// 第三按鈕用的字典
        /// </summary>
        private Dictionary<string, List<StockBuySellOver>> DgvTop50Dict = new Dictionary<string, List<StockBuySellOver>>();

        /// <summary>
        /// 給第一個dgv用的list
        /// </summary>
        private List<Stock> DgvFileShowList = new List<Stock>();

        /// <summary>
        /// 給第二個dgv用的list
        /// </summary>
        private List<StockInfo> DgvSelectList = new List<StockInfo>();

        /// <summary>
        /// 給combobox用的hashset
        /// </summary>
        private static HashSet<string> ComboUseSet = new HashSet<string>();

        /// <summary>
        /// 存放所有stockID的hashset
        /// </summary>
        private static HashSet<string> DgvTotalDictKeys = new HashSet<string>();

        /// <summary>
        /// 串列的第一個位置
        /// </summary>
        private int FirstPlaceIndex = 0;

        /// <summary>
        /// 背景運算
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">记录事件传递过来的额外信息</param>
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 第一個按鈕
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">记录事件传递过来的额外信息</param>
        private void BntReadFile_Click(object sender, EventArgs e)
        {
            string filePath = ReadFileShowPathOnTextBox();

            int fileStatusSignal = -1;
            ChangeLableState(fileStatusSignal += 1);
            File fileRead = new File { Path = filePath };

            List<string> comboDataSourcelist = new List<string>();

            Task taskMakeFirstDict = new Task(() =>
            {
                StopWatch.Start();
                ReadfileToDgvFileShowAndCombobox(fileRead);
                StopWatch.Stop();
                
            });
            Task taskMakeCombobox = new Task(() =>
            {
                StopWatch.Start();
                comboDataSourcelist = ComboUseSet.ToList();
                ComboBox.BeginInvoke(new Action(
                    delegate ()
                    {
                        ComboBox.DataSource = comboDataSourcelist;
                    }
                ));
                StopWatch.Stop();
            });

            taskMakeFirstDict.Start();
            taskMakeFirstDict.GetAwaiter().OnCompleted(() =>
            {
                ChangeLableState(fileStatusSignal += 1);
                TxtReadStatus.Text = AllText += "檔案處理時間: " + GetTimeSpan(StopWatch);
                StopWatch.Reset();
                taskMakeCombobox.Start();
            });
            
            taskMakeCombobox.GetAwaiter().OnCompleted(() =>
            {
                TxtReadStatus.Text = AllText += "Combobox產生時間: " + GetTimeSpan(StopWatch);
                StopWatch.Reset();
            });
        }

        /// <summary>
        /// 第二顆按鈕
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件数据</param>
        private void BntStockSearch_Click(object sender, EventArgs e)
        {
            StopWatch.Start();
            if (DgvSearchDict.Keys.Count() == 0)
            {
                BuildDgvSearchDict();
            }

            SearchDgvSearchDict();
            StopWatch.Stop();
            TxtReadStatus.Text = AllText += "查詢時間: " + GetTimeSpan(StopWatch);
            StopWatch.Reset();
        }

        /// <summary>
        /// 第三顆按鈕
        /// </summary>
        /// <param name="sender">pressbuttontrigger</param>
        /// <param name="e">EVENT</param>
        private void BntTop50_Click(object sender, EventArgs e)
        {
            if (DgvTop50Dict.Keys.Count() == 0)
            {
                StopWatch.Start();
                BuildDgvTop50Dict();
                StopWatch.Stop();
            }
            StopWatch.Start();
            List<string> returnCbbSelectIdList = new List<string>();
            returnCbbSelectIdList = GetComboboxInput(ComboBox.SelectedIndex);
            SearchDgvTop50Dict(returnCbbSelectIdList);
            StopWatch.Stop();
            TxtReadStatus.Text = AllText += "Top50產生時間: " + GetTimeSpan(StopWatch);
            StopWatch.Reset();
        }

        /// <summary>
        /// 去第三個按鈕的字典裡撈資料
        /// </summary>
        /// <param name="returnCbbSelectIdList">combobox指定查詢項目</param>
        public void SearchDgvTop50Dict(List<string> returnCbbSelectIdList)
        {
            List<StockBuySellOver> dgvTop50UseList = new List<StockBuySellOver>();

            string token = string.Empty;
            foreach (var item in returnCbbSelectIdList)
            {
                token = item.ToString();

                if (!DgvTop50Dict.ContainsKey(token))
                {
                    break;
                }
                else
                {
                    dgvTop50UseList.AddRange(DgvTop50Dict[token]);
                }
            }
            DgvTop50.DataSource = dgvTop50UseList;
            string[] colInvisDgvTop50 = { "SecBrokerID", "SortedIndex" };
            Dgv_column_invisible(DgvTop50, colInvisDgvTop50);
        }

        /// <summary>
        /// 把不需要顯示出來的行隱藏起來的方法
        /// </summary>
        /// <param name="dgv">目標的datadridview.</param>
        /// <param name="col">指定的目標行</param>
        public void Dgv_column_invisible(DataGridView dgv, string[] col)
        {
            for (int i = 0; i < col.Length; i++)
            {
                dgv.Columns[col[i]].Visible = false;
            }
        }

        /// <summary>
        /// 建構top50的字典
        /// </summary>
        public void BuildDgvTop50Dict()
        {
            List<StockBuySellOver> allTop50List = new List<StockBuySellOver>();
            DgvTop50Dict.Add("All", allTop50List);
            foreach (var  key in DgvTotalDictKeys)
            {
                DgvTop50Dict.Add(key, null);
            }
            //foreach (var dicKey in DgvTotalDictKeys) 9xx---> 5xx
            Parallel.ForEach(DgvTotalDictKeys, dicKey =>
            {
                Dictionary<string, List<StockBuySellOver>> dicKeyTop50Dict = new Dictionary<string, List<StockBuySellOver>>();
                HashSet<string> dicKeyBrokerIdHashSet = new HashSet<string>();
                int sortIndex = 0;
                foreach (var item in DgvReadFileDict[dicKey])
                {
                    if (!dicKeyBrokerIdHashSet.Contains(item.SecBrokerID))
                    {
                        sortIndex++;
                        dicKeyBrokerIdHashSet.Add(item.SecBrokerID);

                        List<StockBuySellOver> indivStockIdBrokerIdCubeList = new List<StockBuySellOver>();
                        StockBuySellOver brokerIdCube = new StockBuySellOver()
                        {
                            StockName = item.StockName,
                            SecBrokerName = item.SecBrokerName,
                            SortedIndex = sortIndex,
                            BuySellOver = item.BuyQty - item.SellQty
                        };
                        indivStockIdBrokerIdCubeList.Add(brokerIdCube);
                        dicKeyTop50Dict.Add(item.SecBrokerID, indivStockIdBrokerIdCubeList);
                    }
                    else
                    {
                        dicKeyTop50Dict[item.SecBrokerID][FirstPlaceIndex].BuySellOver += (item.BuyQty - item.SellQty);
                    }
                }
                MakeCalcualatedListToBeSorttedAndMerge(dicKey, dicKeyTop50Dict);
            });

            foreach (var key in DgvTotalDictKeys)
            {
                DgvTop50Dict["All"].AddRange(DgvTop50Dict[key]);
            }
        }

        /// <summary>
        /// 把中間暫存數據的字典轉成top50list存進DgvTop50Dict
        /// </summary>
        /// <param name="dicKey">StockId</param>
        /// <param name="dicKeyTop50Dict">中間暫存數據的字典</param>
        public void MakeCalcualatedListToBeSorttedAndMerge(string dicKey, Dictionary<string, List<StockBuySellOver>> dicKeyTop50Dict)
        {
            List<StockBuySellOver> dicKeyTop50List = new List<StockBuySellOver>();

            foreach (var item in dicKeyTop50Dict.Keys)
            {
                dicKeyTop50List.Add(dicKeyTop50Dict[item][FirstPlaceIndex]);
            }

            List<StockBuySellOver> copyDicKeyTop50List = dicKeyTop50List.ToList();

            //Task -- parallel creat buttomTopList
            List<StockBuySellOver> buttomToTopList = new List<StockBuySellOver>();
            Task subThread = new Task(() =>
            {
                List<StockBuySellOver> sortedList = new List<StockBuySellOver>();

                sortedList=(Quicksort(dicKeyTop50List));

                buttomToTopList = sortedList.ToList();
            });
            subThread.Start();

            List<StockBuySellOver> topToButtomList = new List<StockBuySellOver>();
            topToButtomList = ParallelSorting(copyDicKeyTop50List);
            subThread.Wait();
            MakeTop50List(topToButtomList, buttomToTopList, dicKey);
        }

        /// <summary>
        /// 平行處理sorting top50(正數)的半串列
        /// </summary>
        /// <param name="collect_list">待處理串列</param>
        /// <returns>從大到小的all list</returns>
        public List<StockBuySellOver> ParallelSorting(List<StockBuySellOver> collectList)
        {
            ////collect_list.Reverse();
            List<StockBuySellOver> forBTSSortedList = new List<StockBuySellOver>();
            ////collect_list.Sort();
            forBTSSortedList = Decquicksort(collectList).ToList();
            forBTSSortedList.Reverse();

            List<StockBuySellOver> topButtomList = forBTSSortedList.ToList();

            return topButtomList;
        }

        /// <summary>
        /// 排序的quicksort (小->大)
        /// </summary>
        /// <param name="list">待sort的list</param>
        /// <returns>排好序的list</returns>
        public List<StockBuySellOver> Quicksort(List<StockBuySellOver> list)
        {
            if (list.Count <= 1)
            {
                return list;
            }

            int pivotPosition = list.Count / 2;

            StockBuySellOver pivot = list[pivotPosition];

            list.RemoveAt(pivotPosition);
            List<StockBuySellOver> smaller = new List<StockBuySellOver>();
            List<StockBuySellOver> greater = new List<StockBuySellOver>();

            foreach (StockBuySellOver item in list)
            {
                if (item.BuySellOver < pivot.BuySellOver)
                {
                    smaller.Add(item);
                }
                else if (item.BuySellOver > pivot.BuySellOver)
                {
                    greater.Add(item);
                }
                else
                {
                    if (item.SortedIndex < pivot.SortedIndex)
                    {
                        smaller.Add(item);
                    }
                    else if (item.SortedIndex > pivot.SortedIndex)
                    {
                        greater.Add(item);
                    }
                }
            }
            List<StockBuySellOver> sorted = Quicksort(smaller);
            sorted.Add(pivot);
            sorted.AddRange(Quicksort(greater));
            return sorted;
        }

        /// <summary>
        /// 排序的quicksort (小(後)-> (先)大)
        /// </summary>
        /// <param name="list">待sort的list</param>
        /// <returns>排好序的list</returns>
        public static List<StockBuySellOver> Decquicksort(List<StockBuySellOver> list)
        {
            if (list.Count <= 1)
            {
                return list;
            }

            int pivotPosition = list.Count / 2;

            StockBuySellOver pivot = list[pivotPosition];

            list.RemoveAt(pivotPosition);
            List<StockBuySellOver> left = new List<StockBuySellOver>();
            List<StockBuySellOver> right = new List<StockBuySellOver>();

            foreach (StockBuySellOver item in list)
            {
               if (item.BuySellOver < pivot.BuySellOver)
               {
                   left.Add(item);
               }
               else if (item.BuySellOver > pivot.BuySellOver)
               {
                   right.Add(item);
               }
               else
               {
                   if (item.SortedIndex > pivot.SortedIndex)
                   {
                       left.Add(item);
                   }
                   else if (item.SortedIndex < pivot.SortedIndex)
                   {
                       right.Add(item);
                   }
               }
            }
            List<StockBuySellOver> sorted = Decquicksort(left);
            sorted.Add(pivot);
            sorted.AddRange(Decquicksort(right));
            return sorted;
        }

        /// <summary>
        /// 用大到小跟小到大的LIST合併成一個TOP50LIST並輸出
        /// </summary>
        /// <param name="top_bottom_all_list">從大到小的list</param>
        /// <param name="bottom_top_all_list">從小到大的list</param>
        /// <param name="key">指定的stockID</param>
        public void MakeTop50List(List<StockBuySellOver> topBottomAllList, List<StockBuySellOver> bottomTopAllList, string key)
        {
            List<StockBuySellOver> top50List = new List<StockBuySellOver>();
            int topbutlistcount = 0;
            while (topbutlistcount < topBottomAllList.Count && topbutlistcount < 50 && topBottomAllList[topbutlistcount].BuySellOver > 0)
            {
                top50List.Add(topBottomAllList[topbutlistcount]);
                topbutlistcount++;
            }

            int buttoplistcount = 0;
            while (buttoplistcount < bottomTopAllList.Count && buttoplistcount < 50 && bottomTopAllList[buttoplistcount].BuySellOver < 0)
            {
                top50List.Add(bottomTopAllList[buttoplistcount]);
                buttoplistcount++;
            }
            DgvTop50Dict[key] = top50List;
        }

        /// <summary>
        /// 建立第二個按鈕要用的字典
        /// </summary>
        public void BuildDgvSearchDict()
        {
            List<StockInfo> allDgvSearchDictList = new List<StockInfo>();
            DgvSearchDict.Add("All", allDgvSearchDictList);
            foreach (var key in DgvReadFileDict.Keys)
            {
                int buySum = 0;
                int sellSum = 0;
                double indivPrice = 0;
                HashSet<string> brokerIdHashSet = new HashSet<string>();
                int firstPositionIndex = 0;
                StockInfo stockInfo = new StockInfo()
                {
                    StockID = key,
                    StockName = DgvReadFileDict[key][firstPositionIndex].StockName
                };

                foreach (var item in DgvReadFileDict[key])
                {
                    buySum += item.BuyQty;
                    sellSum += item.SellQty;
                    indivPrice += (item.BuyQty + item.SellQty) * item.Price;
                    brokerIdHashSet.Add(item.SecBrokerID);
                }
                stockInfo.BuyTotal = buySum;
                stockInfo.SellTotal = sellSum;
                stockInfo.AvgPrice = indivPrice / (buySum + sellSum);
                stockInfo.BuySellOver = buySum - sellSum;
                stockInfo.SecBrokerCnt = brokerIdHashSet.Count;
                List<StockInfo> stockInfoList = new List<StockInfo>();
                stockInfoList.Add(stockInfo);
                DgvSearchDict.Add(key, stockInfoList);
                DgvSearchDict["All"].Add(stockInfo);
            }
            DgvReadFileDict.Add("All", DgvFileShowList);
        }
        
        /// <summary>
        /// 搜尋第二個按鈕的字典
        /// </summary>
        public void SearchDgvSearchDict()
        {
            List<string> tokenList = GetComboboxInput(ComboBox.SelectedIndex);
            DgvSelectList.Clear();
            DgvSelectList = MakeDgvSearchList(tokenList);
            DgvSearch.DataSource = DgvSelectList;
            DgvFileShow.DataSource = MakeDgvFileshowLIist(tokenList);
        }

        /// <summary>
        /// 取得combobox的輸入
        /// </summary>
        /// <param name="selectIndex">是否為文字輸入</param>
        /// <returns>combobox的選項</returns>
        public List<string> GetComboboxInput(int selectIndex)
        {
            List<string> cbbInputList = new List<string>();

            string[] cbbInputTxtArray;
            if (selectIndex == -1)
            {
                string cbb_input_txt;
                cbb_input_txt = ComboBox.Text;

                cbbInputTxtArray = cbb_input_txt.Split(',');
                cbbInputList.AddRange(cbbInputTxtArray.ToList());
            }
            else
            {
                int firstPalceIndex = 0;
                string token = ComboBox.SelectedItem.ToString();
                cbbInputTxtArray = token.Split(new char[] { ' ', '-' });
                cbbInputList.Add(cbbInputTxtArray[firstPalceIndex]);
            }

            return cbbInputList;
        }

        /// <summary>
        /// 跟據拿到的combobox imput 去創建輸入第二視窗的list
        /// </summary>
        /// <param name="cbbInputList">The first name to join.</param>
        /// <returns>輸入第二視窗的list</returns>
        public List<StockInfo> MakeDgvSearchList(List<string> cbbInputList)
        {
            List<StockInfo> SearchList = new List<StockInfo>();
            foreach (var token in cbbInputList)
            {
                if (!DgvReadFileDict.ContainsKey(token))
                {
                    const string message = "please input combobox StockID !";
                    MessageBox.Show(message);

                    break;
                }
                else
                {
                    SearchList.AddRange(DgvSearchDict[token]);
                }
            }
            return SearchList;
        }

        /// <summary> 
        /// 根據combobox的輸入生成dgv_fileread 的list
        /// </summary>
        /// <param name="get_combobox_input">combobox的輸入</param>
        /// <returns>dgv_fileread 的list</returns>
        public List<Stock> MakeDgvFileshowLIist(List<string> comboboxInput)
        {
            List<Stock> dgvFileShowList = new List<Stock>();
            foreach (var token in comboboxInput)
            {
                if (!DgvReadFileDict.ContainsKey(token))
                {
                    break;
                }
                else
                {
                    dgvFileShowList.AddRange(DgvReadFileDict[token]);
                }
            }

            return dgvFileShowList;
        }

        /// <summary>
        /// 定義檔案的物件
        /// </summary>
        public struct File
        {
            /// <summary>
            /// 有一個path
            /// </summary>
            public string Path { get; set; }
        }

        /// <summary>
        /// 讀取檔案並建構dictionary and  combobox用的hashset
        /// </summary>
        /// <param name="file">檔案的路徑.</param>
        public void ReadfileToDgvFileShowAndCombobox(File file)
        {
            ReadFileMakeDictionary(file.Path);
            // dictionary-->list  
            foreach (var item in DgvReadFileDict.Keys)
            {
                DgvFileShowList.AddRange(DgvReadFileDict[item]);
            }

            //list 回傳datagridview (ui)
            PutListOnDgvFileShow(DgvFileShowList);
        }

        /// <summary>
        /// list --> datagrigview
        /// </summary>
        /// <param name="dgvFileShowList">輸出給dgv的list.</param>
        private void PutListOnDgvFileShow(List<Stock> dgvFileShowList)
        {
            if (DgvFileShow.InvokeRequired)
            {
                //updateGridDelegate delegateControl = new updateGridDelegate(list_to_dgv_showfile);
                //dgv_fileshow.Invoke(delegateControl);
                DgvFileShow.BeginInvoke(new Action(
                    delegate ()
                    {
                        DgvFileShow.DataSource = dgvFileShowList;
                    }
                ));
            }
            else
            {
                DgvFileShow.DataSource = dgvFileShowList;
            }
        }

        /// <summary>
        /// 讀取檔案對話視窗抓取檔案路徑並顯示出來
        /// </summary>
        /// <returns>檔案路徑名稱</returns>
        public string ReadFileShowPathOnTextBox()
        {
            OpenFileDialog openReadFileDialog = new OpenFileDialog();

            openReadFileDialog.Title = "請開啟(.csv)檔案";
            string fileName = string.Empty;

            if (openReadFileDialog.ShowDialog() == DialogResult.OK)
            {
                //顯示路徑
                fileName = openReadFileDialog.FileName;
                TxtFilePath.Text = fileName;
            }
            return fileName;
        }

        /// <summary>
        /// 判斷現在是在讀檔案還是已經讀檔完成(顯示在label)
        /// </summary>
        /// <param name="signal">狀態信號</param>
        /// <returns>狀態的信號</returns>
        public int ChangeLableState(int signal)
        {
            if (signal == 0)
            {
                //改寫label
                LabelFileStatus.Text = "讀檔中";
            }
            else if (signal > 0)
            {
                //改寫label
                LabelFileStatus.Text = "讀檔完成";
            }
            return signal;
        }

        /// <summary>
        /// 股票資訊編號
        /// </summary>
        public enum StockSignature
        {
            DealDate = 0,
            StockID = 1,
            StockName = 2,
            SecBrokerID = 3,
            SecBrokerName = 4,
            Price = 5,
            BuyQty = 6,
            SellQty = 7,
        }

        /// <summary>
        /// 讀檔動作類別並讀取檔案創建_字典_和_combo hashset_和_stockID hashset_並回傳tuple
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <returns>一個元組</returns>
        public static void ReadFileMakeDictionary(string filePath)
        {
            ComboUseSet.Add("All");
            StreamReader sr = new StreamReader(filePath);
            sr.ReadLine();
            int EndCheck = 0;
            while (sr.Peek() > EndCheck)
            {
                string line = sr.ReadLine();
                string[] rows = line.Split(',');
                if (!DgvReadFileDict.ContainsKey(rows[(int)StockSignature.StockID]))
                {
                    List<Stock> valuelist = new List<Stock>();
                    Stock stock = new Stock();
                    stock.DealDate = rows[(int)StockSignature.DealDate];
                    stock.StockID = rows[(int)StockSignature.StockID];
                    stock.StockName = rows[(int)StockSignature.StockName];
                    stock.SecBrokerID = rows[(int)StockSignature.SecBrokerID];
                    stock.SecBrokerName = rows[(int)StockSignature.SecBrokerName];
                    stock.Price = double.Parse(rows[(int)StockSignature.Price]);
                    stock.BuyQty = int.Parse(rows[(int)StockSignature.BuyQty]);
                    stock.SellQty = int.Parse(rows[(int)StockSignature.SellQty]);
                    valuelist.Add(stock);
                    DgvReadFileDict.Add(rows[(int)StockSignature.StockID], valuelist);
                    ComboUseSet.Add(rows[(int)StockSignature.StockID] + " - " + rows[(int)StockSignature.StockName]);
                    DgvTotalDictKeys.Add(rows[(int)StockSignature.StockID]);
                }
                else
                {
                    DgvReadFileDict[rows[(int)StockSignature.StockID]].Add(new Stock()
                    {
                        DealDate = rows[(int)StockSignature.DealDate],
                        StockID = rows[(int)StockSignature.StockID],
                        StockName = rows[(int)StockSignature.StockName],
                        SecBrokerID = rows[(int)StockSignature.SecBrokerID],
                        SecBrokerName = rows[(int)StockSignature.SecBrokerName],
                        Price = double.Parse(rows[(int)StockSignature.Price]),
                        BuyQty = int.Parse(rows[(int)StockSignature.BuyQty]),
                        SellQty = int.Parse(rows[(int)StockSignature.SellQty]),
                    });
                }
            }
            //return Tuple.Create( comboUseHashSet, dictKeysHashSet);
        }

        /// <summary>
        /// 定義"看時間區間"的這個動作
        /// </summary>
        /// <param name="stopWatch">計時的表</param>
        /// <returns>需更新的時間text</returns>
        public string GetTimeSpan(Stopwatch stopWatch)
        {
            TimeSpan timeSpan = stopWatch.Elapsed;
            string elapsedTimeCombobox = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            string textReturn = elapsedTimeCombobox + Environment.NewLine;

            return textReturn;
        }

        /// <summary>
        /// 把不需要顯示出來的行隱藏起來的方法
        /// </summary>
        /// <param name="dgv">目標的datadridview.</param>
        /// <param name="col">指定的目標行</param>
        public void MakeDgvColumnInvisible(DataGridView dgv, string[] col)
        {
            for (int i = 0; i < col.Length; i++)
            {
                dgv.Columns[col[i]].Visible = false;
            }
        }

        /// <summary>
        /// combobox
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">记录事件传递过来的额外信息</param>
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 顯示讀檔時間紀錄的textbox
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">记录事件传递过来的额外信息</param>
        private void TxtReadStatus_TextChanged(object sender, EventArgs e)
        {
            TxtReadStatus.ScrollBars = ScrollBars.Vertical;
            TxtReadStatus.SelectionStart = TxtReadStatus.Text.Length;
            TxtReadStatus.ScrollToCaret();
        }

        /// <summary>
        /// 第一個datagridview
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件传递过来的额外信息</param>
        private void DgvFileShow_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 讀取檔案路徑的textbox
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件传递过来的额外信息</param>
        private void TxtFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 顯示讀檔狀態的label
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件数据</param>
        private void LabelFileStatus_Click(object sender, EventArgs e)
        {

        }
    }
}