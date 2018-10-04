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
            }
            _model.NewGame(Convert.ToInt32(_tableSize.Value));
            _ButtonRow2.ColumnCount = _model.GetSize;
            gameButtons = new Button[_model.GetSize];
            for (Int32 i = 0; i < _model.GetSize; i++)
            {
                gameButtons[i] = new Button();
                gameButtons[i].BackColor = Color.Blue;
                gameButtons[i].FlatStyle = FlatStyle.Flat;
                gameButtons[i].Size = new Size(50, 50);
                _ButtonRow2.Controls.Add(gameButtons[i], i, 1);
            }
        }
    }
}
