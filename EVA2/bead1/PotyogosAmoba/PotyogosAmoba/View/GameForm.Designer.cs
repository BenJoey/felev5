﻿namespace PotyogosAmoba
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._GenButton = new System.Windows.Forms.Button();
            this._tableSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._gameDisplayTable = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._XTIMEDISPLAY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._0TIMEDISPLAY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentPlayerDisplay = new System.Windows.Forms.ToolStripStatusLabel();
            this._PauseButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.játékBetöltéseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilépésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._GenButton);
            this.groupBox1.Controls.Add(this._tableSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(91, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Settings";
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
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "A pálya n*n-es n=";
            // 
            // _gameDisplayTable
            // 
            this._gameDisplayTable.AutoSize = true;
            this._gameDisplayTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._gameDisplayTable.ColumnCount = 2;
            this._gameDisplayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._gameDisplayTable.Location = new System.Drawing.Point(12, 63);
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
            this._0TIMEDISPLAY,
            this.toolStripStatusLabel3,
            this.CurrentPlayerDisplay});
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
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(7, 3, 0, 2);
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
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(106, 17);
            this.toolStripStatusLabel3.Text = "Soron lévő játékos:";
            // 
            // CurrentPlayerDisplay
            // 
            this.CurrentPlayerDisplay.Name = "CurrentPlayerDisplay";
            this.CurrentPlayerDisplay.Size = new System.Drawing.Size(0, 17);
            // 
            // _PauseButton
            // 
            this._PauseButton.Location = new System.Drawing.Point(351, 27);
            this._PauseButton.Name = "_PauseButton";
            this._PauseButton.Size = new System.Drawing.Size(75, 23);
            this._PauseButton.TabIndex = 5;
            this._PauseButton.Text = "Pause";
            this._PauseButton.UseVisualStyleBackColor = true;
            this._PauseButton.Visible = false;
            this._PauseButton.Click += new System.EventHandler(this.PauseButtonHandler);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFileSaveGame,
            this.játékBetöltéseToolStripMenuItem,
            this.kilépésToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // _menuFileSaveGame
            // 
            this._menuFileSaveGame.Name = "_menuFileSaveGame";
            this._menuFileSaveGame.Size = new System.Drawing.Size(151, 22);
            this._menuFileSaveGame.Text = "Játék mentése";
            this._menuFileSaveGame.Click += new System.EventHandler(this.SaveGame_Click);
            // 
            // játékBetöltéseToolStripMenuItem
            // 
            this.játékBetöltéseToolStripMenuItem.Name = "játékBetöltéseToolStripMenuItem";
            this.játékBetöltéseToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.játékBetöltéseToolStripMenuItem.Text = "Játék betöltése";
            this.játékBetöltéseToolStripMenuItem.Click += new System.EventHandler(this.LoadGame_Click);
            // 
            // kilépésToolStripMenuItem
            // 
            this.kilépésToolStripMenuItem.Name = "kilépésToolStripMenuItem";
            this.kilépésToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.kilépésToolStripMenuItem.Text = "Kilépés";
            this.kilépésToolStripMenuItem.Click += new System.EventHandler(this.Exit_Click);
            // 
            // _openFileDialog
            // 
            this._openFileDialog.FileName = "openFileDialog1";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._PauseButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._gameDisplayTable);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameForm";
            this.Text = "Potyogós Amőba";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableSize)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown _tableSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _GenButton;
        private System.Windows.Forms.TableLayoutPanel _gameDisplayTable;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel _XTIMEDISPLAY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel _0TIMEDISPLAY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel CurrentPlayerDisplay;
        private System.Windows.Forms.Button _PauseButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _menuFileSaveGame;
        private System.Windows.Forms.ToolStripMenuItem játékBetöltéseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kilépésToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}

