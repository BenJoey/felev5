using System;
using PotyogosAmoba.Persistence;
using PotyogosAmoba.Model;
using System.Threading.Tasks;

namespace PotyogosAmoba.Test
{
    public class AmobaMockData : IAmobaDataAccess
    {
        public async Task<Tuple<Int32, Int32, Int32, Player, Player[,]>> LoadAsync(String path)
        {
            //Konstans játékállással teszteljük a perzisztencia működését
            Int32 gameSize = 11, XTime = 5, YTime = 3;
            Player _current = Player.Player0;
            Player[,] TestGameState = new Player[gameSize, gameSize];
            //Elhelyzünk 3 lépést a játéktáblán
            TestGameState[10, 3] = Player.PlayerX; TestGameState[10, 2] = Player.Player0; TestGameState[10, 7] = Player.PlayerX;
            Tuple<Int32, Int32, Int32, Player, Player[,]> ToRet =  Tuple.Create(gameSize, XTime, YTime, _current, TestGameState);
            return ToRet;
        }

        public async Task SaveAsync(String path, Tuple<Int32, Int32, Int32, Player, Player[,]> input) { }
    }
}
