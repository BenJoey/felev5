using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PotyogosAmoba.Model;

namespace PotyogosAmoba
{
    public partial class GameForm : Form
    {
        private PAmobaModel _model;
        private Label[,] gameBoard;
        private Button[] gameButtons;
        private Timer _timer;

        public GameForm()
        {
            InitializeComponent();

            _model = new PAmobaModel();
        }

        private void Table_Gen_Click(object sender, EventArgs e)
        {
            if(gameButtons != null)
            {
                foreach (Button a in gameButtons)
                    _ButtonRow2.Controls.Remove(a);
                foreach (Label l in gameBoard)
                    _gameDisplayTable.Controls.Remove(l);
            }

            _timer = new Timer();
            _timer.Interval = 1000;

            _model.NewGame(Convert.ToInt32(_tableSize.Value));
            _ButtonRow2.ColumnCount = _model.GetSize;
            _gameDisplayTable.RowCount = _gameDisplayTable.ColumnCount = _model.GetSize;
            gameButtons = new Button[_model.GetSize];
            gameBoard = new Label[_model.GetSize, _model.GetSize];
            for (Int32 i = 0; i < _model.GetSize; i++)
            {
                gameButtons[i] = new Button();
                gameButtons[i].BackColor = Color.Cyan;
                gameButtons[i].FlatStyle = FlatStyle.Flat;
                gameButtons[i].TabIndex = i;
                gameButtons[i].Size = new Size(30, 30);
                _ButtonRow2.Controls.Add(gameButtons[i], i, 1);
                for (int j = 0; j < _model.GetSize; j++)
                {
                    gameBoard[j, i] = new Label();
                    //gameBoard[j, i].Text = "";
                    gameBoard[j, i].BorderStyle = BorderStyle.FixedSingle;
                    gameBoard[j, i].TextAlign = ContentAlignment.MiddleCenter;
                    gameBoard[j, i].Size = new Size(30, 30);
                    gameBoard[j, i].FlatStyle = FlatStyle.Flat;
                    _gameDisplayTable.Controls.Add(gameBoard[j, i], j, i);
                }
            }
        }
    }
}
