using System;
using PotyogosAmoba.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotyogosAmoba.Model
{
    public enum Player { PlayerX, Player0, NoPlayer}

    public class PAmobaModel
    {
        #region Fields

        private IAmobaDataAccess _dataAccess;
        private Int32 gameSize;
        private Player _currentPlayer;
        private Player[,] gameTable;
        private Int32 playerXTime;
        private Int32 player0Time;

        #endregion

        #region Constructors

        public PAmobaModel(IAmobaDataAccess dataA)
        {
            _dataAccess = dataA;
        }
        public PAmobaModel(Int32 size, Int32 xTime, Int32 OTime, Player _curr, Player[,] table)
        {
            gameSize = size;
            playerXTime = xTime;
            player0Time = OTime;
            _currentPlayer = _curr;
            gameTable = table;
        }

        #endregion

        #region Events

        public event EventHandler<AmobaEvent> GameOver;

        #endregion

        #region Properties

        public Int32 GetSize { get { return gameSize; } }

        public Int32 PlXTime { get { return playerXTime; } }

        public Int32 Pl0Time { get { return player0Time; } }

        public Player CurrentPlayer { get { return _currentPlayer; } }

        public Player GetFieldValue(Int32 x, Int32 y)
        {
            if (x < 0 || x > gameSize - 1 || y < 0 || y > gameSize - 1) return Player.NoPlayer; //Érvénytelen mező lekérés esetén üres playert adunk vissza
            return gameTable[x, y];
        }

        #endregion

        #region Public methods

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

        /// <summary>
        /// Játékos karakterének elhelyezése a kattintott oszlopba.
        /// </summary>
        public void Step(Int32 Column)
        {
            Int32 RowInd;
            for (RowInd = 0; RowInd < (gameSize - 1); RowInd++)
            {
                if (gameTable[Column, RowInd + 1] != Player.NoPlayer) break;
            }
            gameTable[Column, RowInd] = _currentPlayer;
            _currentPlayer = _currentPlayer == Player.PlayerX ? Player.Player0 : Player.PlayerX;
        }

        public async Task LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            Tuple<Int32, Int32, Int32, Player, Player[,]> Loaded_data = await _dataAccess.LoadAsync(path);
            gameSize = Loaded_data.Item1;
            playerXTime = Loaded_data.Item2;
            player0Time = Loaded_data.Item3;
            _currentPlayer = Loaded_data.Item4;
            gameTable = Loaded_data.Item5;
        }

        public async Task SaveGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, Tuple.Create(gameSize, playerXTime, player0Time, _currentPlayer, gameTable));
        }

        /// <summary>
        /// Játék vége ellenőrzés.
        /// </summary>
        public void GameCheck()
        {
            bool tie = true;
            for (Int32 i = 0; i < gameSize; i++)
            {
                for (Int32 j = 0; j < gameSize; j++)
                {
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j) && GetFieldValue(i + 1, j) == GetFieldValue(i + 2, j) && GetFieldValue(i + 2, j) == GetFieldValue(i + 3, j))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j), Tuple.Create(i + 2, j), Tuple.Create(i + 3, j) };
                        Console.WriteLine("TEST");
                        GameOver(this, new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j + 1) && GetFieldValue(i + 2, j + 2) == GetFieldValue(i + 3, j + 3) && GetFieldValue(i, j) == GetFieldValue(i + 2, j + 2))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j + 1), Tuple.Create(i + 2, j + 2), Tuple.Create(i + 3, j + 3) };
                        GameOver(this, new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j - 1) && GetFieldValue(i + 2, j - 2) == GetFieldValue(i + 3, j - 3) && GetFieldValue(i, j) == GetFieldValue(i + 2, j - 2))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j - 1), Tuple.Create(i + 2, j - 2), Tuple.Create(i + 3, j - 3) };
                        GameOver(this, new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    tie = tie && GetFieldValue(i, j) != Player.NoPlayer; //Ha egyik mező se üres és nincs nyertes akkor a játék döntetlen
                }
            }
            if (tie) GameOver(this, new AmobaEvent(Player.NoPlayer, playerXTime, player0Time, new Tuple<Int32, Int32>[0]));
        }

        #endregion
    }
}
