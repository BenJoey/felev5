using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PotyogosAmoba.Persistence
{
    /// <summary>
    /// Játék entitás típusa.
    /// </summary>
    class Game
	{
        /// <summary>
        /// Név, egyedi azonosító.
        /// </summary>
        [Key]
		[MaxLength(32)]
		public String Name { get; set; }

	    /// <summary>
	    /// Tábla mérete.
	    /// </summary>
		public Int32 TableSize { get; set; }

	    /// <summary>
	    /// Soron lévő játékos
	    /// </summary>
		public Player CurrentPlayer { get; set; }
        
        /// <summary>
	    /// X játékos ideje
	    /// </summary>
		public Int32 plXTime { get; set; }

        /// <summary>
        /// 0 játékos ideje
        /// </summary>
        public Int32 pl0Time { get; set; }


        /// <summary>
        /// Mentés időpontja.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Játékmezők.
        /// </summary>
        public ICollection<Field> Fields { get; set; }

		public Game()
		{
			Fields = new List<Field>();
			Time = DateTime.Now;
		}
	}
}