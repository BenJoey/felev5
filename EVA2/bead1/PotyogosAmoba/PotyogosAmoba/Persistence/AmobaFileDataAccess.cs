using System;
using System.IO;
using PotyogosAmoba.Model;
using System.Threading.Tasks;

namespace PotyogosAmoba.Persistence
{
    /// <summary>
    /// Amőba fájlkezelő típusa.
    /// </summary>
    public class AmobaFileDataAccess : AmobaDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játékmodell.</returns>
        public async Task<PAmobaModel> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' ');
                    Int32 tableSize = Int32.Parse(numbers[0]);
                    Int32 xTime = Int32.Parse(numbers[1]);
                    Int32 OTime = Int32.Parse(numbers[2]);
                    Player _curr = numbers[3] == "X" ? Player.PlayerX : Player.Player0;
                    Player[,] gametable = new Player[tableSize, tableSize];

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                            gametable[i, j] = numbers[j] == "X" ? Player.PlayerX : numbers[j] == "O" ? Player.Player0 : Player.NoPlayer;
                    }
                    PAmobaModel ToRet = new PAmobaModel(tableSize, xTime, OTime, _curr, gametable);
                    return ToRet;
                }
            }
            catch
            {
                throw new AmobaDataException();
            }
        }
    }
}
