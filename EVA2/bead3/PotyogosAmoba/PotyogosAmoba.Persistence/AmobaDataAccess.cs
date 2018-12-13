using System;
using System.Threading.Tasks;

public enum Player { NoPlayer, PlayerX, Player0 }

namespace PotyogosAmoba.Persistence
{
    public interface IAmobaDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játékadatok.</returns>
        Task<Tuple<Int32, Int32, Int32, Player, Player[,]>> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="input">A fájlba kiírandó játékadatok.</param>
        Task SaveAsync(String path, Tuple<Int32, Int32, Int32, Player, Player[,]> input);
    }
}
