namespace PotyogosAmoba
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._GenButton = new System.Windows.Forms.Button();
            this._tableSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._ButtonRow2 = new System.Windows.Forms.TableLayoutPanel();
            this._gameDisplayTable = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._XTIMEDISPLAY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._0TIMEDISPLAY = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._GenButton);
            this.groupBox1.Controls.Add(this._tableSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(102, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GameSettings";
            // 
            // _GenButton
            // 
            this._GenButton.Location = new System.Drawing.Point(143, 15);
            this._GenButton.Name = "_GenButton";
            this._GenButton.Size = new System.Drawing.Size(108, 23);
            this._GenButton.TabIndex = 2;
            this._GenButton.Text = "Generate Board";
            this._GenButton.UseVisualStyleBackColor = true;
            this._GenButton.Click += new System.EventHandler(this.Table_Gen_Click);
            // 
            // _tableSize
            // 
            this._tableSize.Location = new System.Drawing.Point(93, 18);
            this._tableSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._tableSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._tableSize.Name = "_tableSize";
            this._tableSize.Size = new System.Drawing.Size(44, 20);
            this._tableSize.TabIndex = 1;
            this._tableSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "A pálya n*n-es n=";
            // 
            // _ButtonRow2
            // 
            this._ButtonRow2.AutoSize = true;
            this._ButtonRow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._ButtonRow2.ColumnCount = 1;
            this._ButtonRow2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this._ButtonRow2.Location = new System.Drawing.Point(12, 58);
            this._ButtonRow2.Name = "_ButtonRow2";
            this._ButtonRow2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this._ButtonRow2.RowCount = 1;
            this._ButtonRow2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.Size = new System.Drawing.Size(0, 20);
            this._ButtonRow2.TabIndex = 2;
            // 
            // _gameDisplayTable
            // 
            this._gameDisplayTable.AutoSize = true;
            this._gameDisplayTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._gameDisplayTable.ColumnCount = 2;
            this._gameDisplayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.Location = new System.Drawing.Point(12, 102);
            this._gameDisplayTable.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this._gameDisplayTable.Name = "_gameDisplayTable";
            this._gameDisplayTable.Padding = new System.Windows.Forms.Padding(0, 10, 0, 40);
            this._gameDisplayTable.RowCount = 2;
            this._gameDisplayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.Size = new System.Drawing.Size(0, 50);
            this._gameDisplayTable.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this._XTIMEDISPLAY,
            this.toolStripStatusLabel2,
            this._0TIMEDISPLAY});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(85, 17);
            this.toolStripStatusLabel1.Text = "X játékos ideje:";
            // 
            // _XTIMEDISPLAY
            // 
            this._XTIMEDISPLAY.Name = "_XTIMEDISPLAY";
            this._XTIMEDISPLAY.Size = new System.Drawing.Size(43, 17);
            this._XTIMEDISPLAY.Text = "0:00:00";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(40, 3, 0, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(87, 17);
            this.toolStripStatusLabel2.Text = "O játékos ideje:";
            // 
            // _0TIMEDISPLAY
            // 
            this._0TIMEDISPLAY.Name = "_0TIMEDISPLAY";
            this._0TIMEDISPLAY.Size = new System.Drawing.Size(43, 17);
            this._0TIMEDISPLAY.Text = "0:00:00";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._gameDisplayTable);
            this.Controls.Add(this._ButtonRow2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GameForm";
            this.Text = "Potyogós Amőba";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown _tableSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _GenButton;
        private System.Windows.Forms.TableLayoutPanel _ButtonRow2;
        private System.Windows.Forms.TableLayoutPanel _gameDisplayTable;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel _XTIMEDISPLAY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel _0TIMEDISPLAY;
    }
}

