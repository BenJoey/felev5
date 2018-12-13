using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PotyogosAmoba.Persistence
{
    /// <summary>
    /// Amőba fájlkezelő típusa.
    /// </summary>
    public class AmobaFileDataAccess : IAmobaDataAccess
    {
        private String _saveDirectory;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="saveDirectory">Mentések útvonala.</param>
        public AmobaFileDataAccess(String saveDirectory)
        {
            _saveDirectory = saveDirectory;
        }

        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játékadatok.</returns>
        public async Task<Tuple<Int32, Int32, Int32, Player, Player[,]>> LoadAsync(String name)
        {
            String path = Path.Combine(_saveDirectory, name + ".sav"); // útvonal előállítása
            try
            {
                using (StreamReader reader = new StreamReader(path))
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
                    Tuple<Int32,Int32, Int32, Player, Player[,]> ToRet = Tuple.Create(tableSize, xTime, OTime, _curr, gametable);
                    return ToRet;
                }
            }
            catch
            {
                throw new AmobaDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="OutPut">A fájlba kiírandó játékadatok.</param>
        public async Task SaveAsync(String path, Tuple<Int32, Int32, Int32, Player, Player[,]> OutPut)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    //A mentés fájl első sorába rakjuk a pályamérete, a két játékos idejét valamint az éppen soron lévő játékost
                    writer.Write(OutPut.Item1); // kiírjuk a méreteket
                    await writer.WriteLineAsync(" " + OutPut.Item2 + " " + OutPut.Item3 + (OutPut.Item4 == Player.PlayerX ? " X" : " O"));
                    for (Int32 i = 0; i < OutPut.Item1; i++)
                    {
                        for (Int32 j = 0; j < OutPut.Item1; j++)
                        {
                            // A fájlba X-el jelöljük az X játékos mezőit, O-val az O kátékosét és E-vel az üres mezőket
                            String ToWri = OutPut.Item5[i, j] == Player.PlayerX ? "X " : OutPut.Item5[i, j] == Player.Player0 ? "O " : "E ";
                            await writer.WriteAsync(ToWri);
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new AmobaDataException();
            }
        }

        /// <summary>
	    /// Játékállapot mentések lekérdezése.
	    /// </summary>
		public async Task<ICollection<SaveEntry>> ListAsync()
        {
            try
            {
                return Directory.GetFiles(_saveDirectory, "*.sav")
                    .Select(path => new SaveEntry
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        Time = File.GetCreationTime(path)
                    })
                    .ToList();
            }
            catch
            {
                throw new AmobaDataException();
            }
        }
    }
}
