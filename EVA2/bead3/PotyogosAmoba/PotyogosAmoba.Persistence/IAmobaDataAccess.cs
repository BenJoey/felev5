using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum Player { NoPlayer, PlayerX, Player0 }

namespace PotyogosAmoba.Persistence
{
    public interface IAmobaDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="name">Mentés neve.</param>
        /// <returns>A fájlból beolvasott játékadatok.</returns>
        Task<Tuple<Int32, Int32, Int32, Player, Player[,]>> LoadAsync(String name);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="name">Mentés neve.</param>
        /// <param name="input">A fájlba kiírandó játékadatok.</param>
        Task SaveAsync(String name, Tuple<Int32, Int32, Int32, Player, Player[,]> input);

        /// <summary>
        /// Játékállapot mentések lekérdezése.
        /// </summary>
	    Task<ICollection<SaveEntry>> ListAsync();
    }
}
