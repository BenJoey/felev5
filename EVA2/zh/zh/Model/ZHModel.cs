using System;
using System.Threading.Tasks;

namespace ZH.Model
{
    public enum Mezo { Hallgato, Nothing, Nyilt, Zart}

    public class ZHModel
    {
        #region Fields
        private Int32 gameSize; //pályaméret
        private Int32[,] DoorTimes;
        private Mezo[,] gameTable; //játéktábla
        int visitedKonzis;
        Int32 Time;
        private Int32 PlayerX;
        private Int32 PlayerY;
        Random r;

        #endregion

        #region Constructors
        public ZHModel()
        {
            //_dataAccess = dataA;
            r = new Random();
        }

        #endregion

        #region Events

        public event EventHandler Refresh;
        public event EventHandler Reset;

        #endregion

        #region Properties
        public Int32 GetSize { get { return gameSize; } }

        public Int32 GetTime { get { return Time; } }

        public Int32 KonziNum { get { return visitedKonzis; } }
        public Mezo GetFieldValue(Int32 x, Int32 y)
        {
            if (x < 0 || x > 4 || y < 0 || y > gameSize - 1) return Mezo.Nothing;
            return gameTable[x, y];
        }

        #endregion

        #region Public methods
        public void NewGame(Int32 size)
        {
            gameSize = size;
            gameTable = new Mezo[5, gameSize];
            DoorTimes = new Int32[5, gameSize];
            visitedKonzis = 0;
            Time = 0;
            PlayerX = 2; PlayerY = size / 2;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < gameSize; j++)
                {
                    DoorTimes[i, j] = 0;
                    if ((i == 0 || i == 4) && j % 2 == 0)
                    {
                        int t = r.Next(2, 4);
                        gameTable[i, j] = (Mezo)t;
                        if (t == 2) DoorTimes[i, j] = 3;
                    }
                    else { gameTable[i, j] = Mezo.Nothing; }
                }
            gameTable[PlayerX, PlayerY] = Mezo.Hallgato;
            Reset_Signal();
        }
        
        public Boolean IsFieldClickable(int x, int y)
        {
            if (Math.Abs(x - PlayerX) == 0 && Math.Abs(y - PlayerY) == 1) return true;
            if (Math.Abs(x - PlayerX) == 1 && Math.Abs(y - PlayerY) == 0) return true;
            return false;
        }

        public void Step(Int32 Row, Int32 Column)
        {
            if (gameTable[Row, Column] == Mezo.Nyilt)
            {
                visitedKonzis++;
                gameTable[Row, Column] = Mezo.Zart;
            }
            else if (gameTable[Row, Column] != Mezo.Zart)
            {
                gameTable[PlayerX, PlayerY] = Mezo.Nothing;
                gameTable[Row, Column] = Mezo.Hallgato;
                PlayerX = Row; PlayerY = Column;
            }
        }

        public void AdvanceTime() {
            Time++;
            if (Time % 2 == 0) { AdvanceTime_OpenDoors(); }
            AdvanceTime_CloseDoors();
            Refresh_Signal();
        }

        #endregion

        #region Private methods

        private void AdvanceTime_CloseDoors()
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < gameSize; j++)
                {
                    if (DoorTimes[i, j] > 0)
                    {
                        DoorTimes[i, j] -= 1;
                        if (DoorTimes[i, j] == 0) gameTable[i, j] = Mezo.Zart;
                    }
                }
        }
        
        private void AdvanceTime_OpenDoors()
        {
            Tuple<Int32, Int32>[] fields = new Tuple<int, int>[gameSize + 1];
            int col = 0;
            for (int i = 0; i < fields.Length; i++)
            {
                if (col >= gameSize) col = 0;
                int row = i <= gameSize / 2 + 1 ? 0 : 4;
                fields[i] = Tuple.Create(row, col);
                col += 2;
            }
            int t = r.Next(1, fields.Length);
            for (int i = 0; i < t; i++)
            {
                int s = r.Next(0, fields.Length);
                Tuple<Int32, Int32> curr = fields[s];
                if (gameTable[curr.Item1, curr.Item2] == Mezo.Zart)
                {
                    gameTable[curr.Item1, curr.Item2] = Mezo.Nyilt;
                    DoorTimes[curr.Item1, curr.Item2] = 4;
                }
            }
        }

        #endregion

        #region Private event signal methods
        private void Refresh_Signal()
        {
            if(Refresh != null)
                Refresh(this, new EventArgs());
        }

        /// <summary>
        /// Játétábla újragenerálásának eseményének kiváltása.
        /// </summary>
        private void Reset_Signal()
        {
            if (Reset != null)
                Reset(this, new EventArgs());
        }

        #endregion
    }
}
