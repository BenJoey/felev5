using System;
using System.Data.Entity;

namespace PotyogosAmoba.Persistence
{
    /// <summary>
    /// Adatbázis kontextus típusa.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
	class AmobaContext : DbContext
	{
		public AmobaContext(String connection)
			: base(connection)
		{
		}

		public DbSet<Game> Games { get; set; }
		public DbSet<Field> Fields { get; set; }
	}
}