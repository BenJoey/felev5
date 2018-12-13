using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PotyogosAmoba.Persistence
{
    /// <summary>
    /// PotyogosAmoba perzisztencia adatbáziskezelő típusa.
    /// </summary>
	public class AmobaDbDataAccess : IAmobaDataAccess
    {
		private AmobaContext _context;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="connection">Adatbázis connection string.</param>
        public AmobaDbDataAccess(String connection)
		{
			_context = new AmobaContext(connection);
			_context.Database.CreateIfNotExists(); // adatbázis séma létrehozása, ha nem létezik
		}

	    /// <summary>
	    /// Játékállapot betöltése.
	    /// </summary>
	    /// <param name="name">Név vagy elérési útvonal.</param>
	    /// <returns>A beolvasott játéktábla.</returns>
		public async Task<Tuple<Int32, Int32, Int32, Player, Player[,]>> LoadAsync(String name)
		{
			try
			{
				Game game = await _context.Games
				    .Include(g => g.Fields)
				    .SingleAsync(g => g.Name == name); // játék állapot lekérdezése
                
                Player[,] loadedtable = new Player[game.TableSize, game.TableSize];

                foreach (Field field in game.Fields) // mentett mezők feldolgozása
				{
					loadedtable[field.X, field.Y]= field.Value;
				}

				return Tuple.Create(game.TableSize, game.plXTime, game.pl0Time, game.CurrentPlayer, loadedtable);
			}
			catch
			{
				throw new AmobaDataException();
			}
		}

	    /// <summary>
	    /// Játékállapot mentése.
	    /// </summary>
	    /// <param name="name">Név vagy elérési útvonal.</param>
	    /// <param name="table">A kiírandó játéktábla.</param>
		public async Task SaveAsync(String name, Tuple<Int32, Int32, Int32, Player, Player[,]> toSave)
		{
			try
			{
                // játékmentés keresése azonos névvel
			    Game overwriteGame = await _context.Games
			        .Include(g => g.Fields)
			        .SingleOrDefaultAsync(g => g.Name == name);
			    if (overwriteGame != null)
			        _context.Games.Remove(overwriteGame); // törlés

				Game dbGame = new Game
				{
					TableSize = toSave.Item1,
					plXTime = toSave.Item2,
                    pl0Time = toSave.Item3,
                    CurrentPlayer = toSave.Item4,
					Name = name
				}; // új mentés létrehozása

				for (Int32 i = 0; i < toSave.Item1; ++i)
				{
					for (Int32 j = 0; j < toSave.Item1; ++j)
					{
						Field field = new Field
						{
							X = i,
							Y = j,
							Value = toSave.Item5[i,j]
						};
						dbGame.Fields.Add(field);
					}
				} // mezők mentése

				_context.Games.Add(dbGame); // mentés hozzáadása a perzisztálandó objektumokhoz
				await _context.SaveChangesAsync(); // mentés az adatbázisba
			}
			catch(Exception ex)
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
	            return await _context.Games
	                .OrderByDescending(g => g.Time) // rendezés mentési idő szerint csökkenő sorrendben
	                .Select(g => new SaveEntry {Name = g.Name, Time = g.Time}) // leképezés: Game => SaveEntry
	                .ToListAsync();
	        }
	        catch
	        {
	            throw new AmobaDataException();
	        }
	    }
	}
}
