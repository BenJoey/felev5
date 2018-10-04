using System;
using PotyogosAmoba.Model;
using System.Threading.Tasks;

namespace PotyogosAmoba.Persistence
{
    public interface AmobaDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<PAmobaModel> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="input">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String path, PAmobaModel input);
    }
}
