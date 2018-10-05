using System;
using PotyogosAmoba.Persistence;
using System.Threading.Tasks;

namespace PotyogosAmoba.Model
{
    public enum Player { PlayerX, Player0, NoPlayer}

    public class PAmobaModel
    {
        #region Fields

        private IAmobaDataAccess _dataAccess; //adatelérés
        private Int32 gameSize; //pályaméret
        private Player _currentPlayer; //soron lévő játékos
        private Player[,] gameTable; //játéktábla
        private Int32 playerXTime; //X játékos ideje
        private Int32 player0Time; //0 játékos ideje

        #endregion

        #region Constructors

        /// <summary>
        /// Amőba modell játék példányosítása.
        /// </summary>
        /// <param name="dataA">Az adatelérés.</param>
        public PAmobaModel(IAmobaDataAccess dataA)
        {
            _dataAccess = dataA;
        }

        #endregion

        #region Events

        public event EventHandler<AmobaEvent> GameOver;
        public event EventHandler RefreshBoard;

        #endregion

        #region Properties

        /// <summary>
        /// Pályaméret lekérdezése.
        /// </summary>
        public Int32 GetSize { get { return gameSize; } }

        /// <summary>
        /// X játékos játékidejének lekérdezése.
        /// </summary>
        public Int32 PlXTime { get { return playerXTime; } }

        /// <summary>
        /// 0 játékos játékidejének lekérdezése.
        /// </summary>
        public Int32 Pl0Time { get { return player0Time; } }

        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public Player CurrentPlayer { get { return _currentPlayer; } }

        /// <summary>
        /// Játéktábla egy mezőjének lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        public Player GetFieldValue(Int32 x, Int32 y)
        {
            //Érvénytelen mező lekérés esetén üres playert adunk vissza (GameOver ellenőrzést teszi egyszerűbbé)
            if (x < 0 || x > gameSize - 1 || y < 0 || y > gameSize - 1) return Player.NoPlayer;
            return gameTable[x, y];
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Új játék kezdése.
        /// </summary>
        /// <param name="size">Kiválasztott pályaméret.</param>
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
        /// <param name="Column">A kattintott oszlop indexe.</param>
        public void Step(Int32 Column)
        {
            Int32 RowInd;
            for (RowInd = 0; RowInd < (gameSize - 1); RowInd++)
            {
                if (gameTable[Column, RowInd + 1] != Player.NoPlayer) break;
            }
            gameTable[Column, RowInd] = _currentPlayer;
            _currentPlayer = _currentPlayer == Player.PlayerX ? Player.Player0 : Player.PlayerX;
            Refresh_Signal();
            GameCheck();
        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
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

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, Tuple.Create(gameSize, playerXTime, player0Time, _currentPlayer, gameTable));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Játék vége ellenőrzés.
        /// </summary>
        private void GameCheck()
        {
            bool tie = true;
            for (Int32 i = 0; i < gameSize; i++)
            {
                for (Int32 j = 0; j < gameSize; j++)
                {
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j) && GetFieldValue(i + 1, j) == GetFieldValue(i + 2, j) && GetFieldValue(i + 2, j) == GetFieldValue(i + 3, j))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j), Tuple.Create(i + 2, j), Tuple.Create(i + 3, j) };
                        GameOver_Signal(new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j + 1) && GetFieldValue(i + 2, j + 2) == GetFieldValue(i + 3, j + 3) && GetFieldValue(i, j) == GetFieldValue(i + 2, j + 2))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j + 1), Tuple.Create(i + 2, j + 2), Tuple.Create(i + 3, j + 3) };
                        GameOver_Signal(new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    if (GetFieldValue(i, j) != Player.NoPlayer && GetFieldValue(i, j) == GetFieldValue(i + 1, j - 1) && GetFieldValue(i + 2, j - 2) == GetFieldValue(i + 3, j - 3) && GetFieldValue(i, j) == GetFieldValue(i + 2, j - 2))
                    {
                        Tuple<Int32, Int32>[] WinningPlace = { Tuple.Create(i, j), Tuple.Create(i + 1, j - 1), Tuple.Create(i + 2, j - 2), Tuple.Create(i + 3, j - 3) };
                        GameOver_Signal(new AmobaEvent(GetFieldValue(i, j), playerXTime, player0Time, WinningPlace));
                    }
                    //Ha egyik mező se üres és nincs nyertes akkor a játék döntetlen és a NoPlayer-t adjuk meg nyertesként
                    tie = tie && GetFieldValue(i, j) != Player.NoPlayer;
                }
            }
            if (tie) GameOver_Signal(new AmobaEvent(Player.NoPlayer, playerXTime, player0Time, new Tuple<Int32, Int32>[0]));
        }

        #endregion

        #region Private event signal methods

        /// <summary>
        /// Játétábla újraírásának eseményének kiváltása.
        /// </summary>
        private void Refresh_Signal()
        {
            if(RefreshBoard != null)
                RefreshBoard(this, new EventArgs());
        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="e">Amőba esemény típus.</param>
        private void GameOver_Signal(AmobaEvent e)
        {
            if (GameOver != null)
                GameOver(this, e);
        }

        #endregion
    }
}
