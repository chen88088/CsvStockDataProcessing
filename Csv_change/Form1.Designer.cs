namespace CsvChange
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BntReadFile = new System.Windows.Forms.Button();
            this.LabelFileStatus = new System.Windows.Forms.Label();
            this.TxtFilePath = new System.Windows.Forms.TextBox();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.BntStockSearch = new System.Windows.Forms.Button();
            this.BntTop50 = new System.Windows.Forms.Button();
            this.TxtReadStatus = new System.Windows.Forms.TextBox();
            this.DgvFileShow = new System.Windows.Forms.DataGridView();
            this.DgvSearch = new System.Windows.Forms.DataGridView();
            this.DgvTop50 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFileShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTop50)).BeginInit();
            this.SuspendLayout();
            // 
            // BntReadFile
            // 
            this.BntReadFile.Location = new System.Drawing.Point(388, 28);
            this.BntReadFile.Name = "BntReadFile";
            this.BntReadFile.Size = new System.Drawing.Size(75, 23);
            this.BntReadFile.TabIndex = 0;
            this.BntReadFile.Text = "讀取檔案";
            this.BntReadFile.UseVisualStyleBackColor = true;
            this.BntReadFile.Click += new System.EventHandler(this.BntReadFile_Click);
            // 
            // LabelFileStatus
            // 
            this.LabelFileStatus.AutoSize = true;
            this.LabelFileStatus.Font = new System.Drawing.Font("新細明體", 11F);
            this.LabelFileStatus.Location = new System.Drawing.Point(498, 28);
            this.LabelFileStatus.Name = "LabelFileStatus";
            this.LabelFileStatus.Size = new System.Drawing.Size(67, 15);
            this.LabelFileStatus.TabIndex = 1;
            this.LabelFileStatus.Text = "讀檔狀態";
            this.LabelFileStatus.Click += new System.EventHandler(this.LabelFileStatus_Click);
            // 
            // TxtFilePath
            // 
            this.TxtFilePath.Location = new System.Drawing.Point(13, 28);
            this.TxtFilePath.Name = "TxtFilePath";
            this.TxtFilePath.Size = new System.Drawing.Size(369, 22);
            this.TxtFilePath.TabIndex = 2;
            this.TxtFilePath.TextChanged += new System.EventHandler(this.TxtFilePath_TextChanged);
            // 
            // ComboBox
            // 
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(13, 57);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(369, 20);
            this.ComboBox.TabIndex = 3;
            this.ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // BntStockSearch
            // 
            this.BntStockSearch.Location = new System.Drawing.Point(389, 53);
            this.BntStockSearch.Name = "BntStockSearch";
            this.BntStockSearch.Size = new System.Drawing.Size(75, 23);
            this.BntStockSearch.TabIndex = 4;
            this.BntStockSearch.Text = "股票查詢";
            this.BntStockSearch.UseVisualStyleBackColor = true;
            this.BntStockSearch.Click += new System.EventHandler(this.BntStockSearch_Click);
            // 
            // BntTop50
            // 
            this.BntTop50.Location = new System.Drawing.Point(470, 53);
            this.BntTop50.Name = "BntTop50";
            this.BntTop50.Size = new System.Drawing.Size(95, 23);
            this.BntTop50.TabIndex = 5;
            this.BntTop50.Text = "買賣超Top50";
            this.BntTop50.UseVisualStyleBackColor = true;
            this.BntTop50.Click += new System.EventHandler(this.BntTop50_Click);
            // 
            // TxtReadStatus
            // 
            this.TxtReadStatus.Location = new System.Drawing.Point(586, 12);
            this.TxtReadStatus.Multiline = true;
            this.TxtReadStatus.Name = "TxtReadStatus";
            this.TxtReadStatus.Size = new System.Drawing.Size(202, 100);
            this.TxtReadStatus.TabIndex = 6;
            this.TxtReadStatus.TextChanged += new System.EventHandler(this.TxtReadStatus_TextChanged);
            // 
            // DgvFileShow
            // 
            this.DgvFileShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvFileShow.Location = new System.Drawing.Point(12, 118);
            this.DgvFileShow.Name = "DgvFileShow";
            this.DgvFileShow.RowTemplate.Height = 24;
            this.DgvFileShow.Size = new System.Drawing.Size(450, 186);
            this.DgvFileShow.TabIndex = 7;
            this.DgvFileShow.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvFileShow_CellContentClick);
            // 
            // DgvSearch
            // 
            this.DgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSearch.Location = new System.Drawing.Point(13, 310);
            this.DgvSearch.Name = "DgvSearch";
            this.DgvSearch.RowTemplate.Height = 24;
            this.DgvSearch.Size = new System.Drawing.Size(451, 129);
            this.DgvSearch.TabIndex = 8;
            // 
            // DgvTop50
            // 
            this.DgvTop50.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTop50.Location = new System.Drawing.Point(470, 118);
            this.DgvTop50.Name = "DgvTop50";
            this.DgvTop50.RowTemplate.Height = 24;
            this.DgvTop50.Size = new System.Drawing.Size(318, 320);
            this.DgvTop50.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DgvTop50);
            this.Controls.Add(this.DgvSearch);
            this.Controls.Add(this.DgvFileShow);
            this.Controls.Add(this.TxtReadStatus);
            this.Controls.Add(this.BntTop50);
            this.Controls.Add(this.BntStockSearch);
            this.Controls.Add(this.ComboBox);
            this.Controls.Add(this.TxtFilePath);
            this.Controls.Add(this.LabelFileStatus);
            this.Controls.Add(this.BntReadFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvFileShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTop50)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BntReadFile;
        private System.Windows.Forms.Label LabelFileStatus;
        private System.Windows.Forms.TextBox TxtFilePath;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.Button BntStockSearch;
        private System.Windows.Forms.Button BntTop50;
        private System.Windows.Forms.TextBox TxtReadStatus;
        private System.Windows.Forms.DataGridView DgvFileShow;
        private System.Windows.Forms.DataGridView DgvSearch;
        private System.Windows.Forms.DataGridView DgvTop50;
    }
}
