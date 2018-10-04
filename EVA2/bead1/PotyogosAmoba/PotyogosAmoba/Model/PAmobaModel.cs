using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotyogosAmoba.Model
{
    public enum Player { PlayerX, Player0, NoPlayer}

    class PAmobaModel
    {
        #region Fields

        private Int32 gameSize;
        private Player _currentPlayer;
        private Player[,] gameTable;
        private Int32 playerXTime;
        private Int32 player0Time;

        #endregion

        #region Properties

        public Int32 GetSize { get { return gameSize; } }

        public Int32 PlXTime { get { return playerXTime; } }

        public Int32 Pl0Time { get { return player0Time; } }

        public Player CurrentPlayer { get { return _currentPlayer; } }

        #endregion

        #region Public game methods

        /// <summary>
        /// Új játék kezdése.
        /// </summary>
        public void NewGame(Int32 size)
        {
            playerXTime = 0;
            player0Time = 0;
            _currentPlayer = Player.PlayerX;
            gameSize = size;
            gameTable = new Player[gameSize, gameSize];
            for (int i = 0; i < gameSize; i++)
            {
                for (int j = 0; j < gameSize; j++)
                {
                    gameTable[i, j] = Player.NoPlayer;
                }
            }
        }

        /// <summary>
        /// Játékidő léptetése.
        /// </summary>
        public void AdvanceTime()
        {
            switch (_currentPlayer)
            {
                case Player.PlayerX:
                    playerXTime++;
                    break;
                case Player.Player0:
                    player0Time++;
                    break;
            }
        }

        public void Step(Int32 x, Int32 y)
        {
            gameTable[x, y] = _currentPlayer;
            _currentPlayer = (Player)((int)(_currentPlayer++) % 2);
        }

        #endregion
    }
}
