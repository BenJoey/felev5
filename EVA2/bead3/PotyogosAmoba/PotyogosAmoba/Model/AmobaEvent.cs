using System;

namespace PotyogosAmoba.Model
{
    /// <summary>
    /// Amőba eseményargumentum típusa.
    /// </summary>
    public class AmobaEvent : EventArgs
    {
        private Int32 playerXTime;
        private Int32 player0Time;
        private Player _winner;
        private Tuple<Int32, Int32>[] WinningPlace;

        /// <summary>
        /// X játékos idejének lekérdezése.
        /// </summary>
        public Int32 GetXTime { get { return playerXTime; } }

        /// <summary>
        /// 0 játékos idejének lekérdezése.
        /// </summary>
        public Int32 Get0Time { get { return player0Time; } }

        /// <summary>
        /// Győztes lekérdezése.
        /// </summary>
        public Player WhoWon { get { return _winner; } }

        /// <summary>
        /// Nyerő karakterek helyének lekérdezése
        /// </summary>
        public Tuple<Int32,Int32>[] WinPlace  { get { return WinningPlace; } }

        /// <summary>
        /// Amőba eseményargumentum példányosítása.
        /// </summary>
        /// <param name="win">Győztes.</param>
        /// <param name="playerX">X játékideje.</param>
        /// <param name="player0">0 játékideje.</param>
        /// <param name="place">Nyerő karakterek helye.</param>
        public AmobaEvent(Player win, Int32 playerX, Int32 player0, Tuple<Int32, Int32>[] place)
        {
            player0Time = player0;
            playerXTime = playerX;
            _winner = win;
            WinningPlace = place;
        }
    }
}
