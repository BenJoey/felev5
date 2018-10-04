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
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).BeginInit();
            this._ButtonRow2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._GenButton);
            this.groupBox1.Controls.Add(this._tableSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(225, 3);
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
            this._ButtonRow2.RowCount = 1;
            this._ButtonRow2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._ButtonRow2.Size = new System.Drawing.Size(6, 6);
            this._ButtonRow2.TabIndex = 2;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._ButtonRow2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GameForm";
            this.Text = "Potyogós Amőba";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).EndInit();
            this._ButtonRow2.ResumeLayout(false);
            this._ButtonRow2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown _tableSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _GenButton;
        private System.Windows.Forms.TableLayoutPanel _ButtonRow2;
    }
}

