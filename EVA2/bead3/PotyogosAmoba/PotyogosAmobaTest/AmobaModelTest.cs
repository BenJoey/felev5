using System;
using PotyogosAmoba.Model;
using PotyogosAmoba.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PotyogosAmoba.Test
{
    [TestClass]
    public class AmobaModelTest
    {
        private PAmobaModel _model;

        [TestInitialize]
        public void Initialize()
        {
            _model = new PAmobaModel(new AmobaMockData()); //A mock adatelérés objektummal inicializáljuk a modelt

            _model.Refresh += new EventHandler(Model_RefreshEvent);
            _model.GameOver += new EventHandler<AmobaEvent>(Model_GameOverEvent);
        }

        #region Model Tests

        [TestMethod]
        public void AmobaModel_NewGameTest()
        {
            _model.NewGame(10);

            Assert.AreEqual(0, _model.PlXTime);
            Assert.AreEqual(Player.PlayerX, _model.CurrentPlayer);
            Assert.AreEqual(10, _model.GetSize);

            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                    Assert.AreEqual(Player.NoPlayer, _model.GetFieldValue(i, j));
        }

        [TestMethod]
        public void AmobaModel_StepTest()
        {
            _model.NewGame(10);
            Assert.AreEqual(Player.PlayerX, _model.CurrentPlayer);

            _model.AdvanceTime();
            _model.Step(7, 9); //a hetedik oszlop, utolsó elemére "kattintunk"

            Assert.AreEqual(Player.Player0, _model.CurrentPlayer);  //megnézzük, hogy váltott-e a másik játékosra
            Assert.AreEqual(Player.PlayerX, _model.GetFieldValue(7, 9)); //elhelyezte-e az X-et a 7. oszlop aljára

            // csinálunk még 4 random lépést
            //a játék itt még nem érhet véget mert 5 lépés alatt 3 X és 2 0 volt
            for (Int32 i = 0; i < 4; i++)
            {
                _model.AdvanceTime();
                _model.Step(i, 9);
            }
            Int32 filled = 0;
            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                    if (_model.GetFieldValue(i, j) != Player.NoPlayer) filled++;

            Assert.AreEqual(5, filled); //5 lépés után annyi mező van-e kitöltve
        }

        [TestMethod]
        public void AmobaModel_AdvanceTimeTest()
        {
            _model.NewGame(10);
            for (Int32 i = 0; i < 5; i++)
                _model.AdvanceTime(); //5-ször léptetjük X idejét

            //majd lépünk egyet, hogy játékost váltsunk
            _model.Step(2, 9);
            for (Int32 i = 0; i < 8; i++)
                _model.AdvanceTime(); //8-szor léptetjük 0 idejét

            //Ellenőrizzük a játékosok eddig eltelt idejét
            Assert.AreEqual(5, _model.PlXTime);
            Assert.AreEqual(8, _model.Pl0Time);
        }

        [TestMethod]
        public void AmobaModel_GameOverTest()
        {
            _model.NewGame(10);
            //Vizszintesen haladva elhelyezünk egy X-et és egy 0-t minden oszlopba
            //A 4.hez érve az X megynerte a játékot
            for (Int32 i = 0; i < 4; i++)
            {
                _model.AdvanceTime();
                _model.Step(i, 9);
                _model.Step(i, 9);
            }
        }

        #endregion

        #region Persistence Test

        [TestMethod]
        public void LoadGameTest()
        {
            _model.LoadGame("");
            //Megnézzük, hogy a MockData-ban lévő adatok kerültek-e a modellbe
            Assert.AreEqual(11, _model.GetSize);
            Assert.AreEqual(Player.Player0, _model.CurrentPlayer);
            Assert.AreEqual(3, _model.Pl0Time);
            Assert.AreEqual(5, _model.PlXTime);
            Int32 XFields = 0, OFields = 0;
            for (Int32 i = 0; i < _model.GetSize; i++)
                for (Int32 j = 0; j < _model.GetSize; j++)
                    switch(_model.GetFieldValue(i,j))
                    {
                        case Player.PlayerX:
                            XFields++;break;
                        case Player.Player0:
                            OFields++;break;
                    }
            Assert.AreEqual(2, XFields);
            Assert.AreEqual(1, OFields);
        }

        #endregion

        #region Model Event handlers

        private void Model_RefreshEvent(Object sender, EventArgs e)
        {
            Assert.IsTrue((_model.Pl0Time > 0 || _model.PlXTime > 0)); //valamelyik játékosnak telt már az ideje
        }

        private void Model_GameOverEvent(Object sender, AmobaEvent e)
        {
            Assert.AreEqual(4, e.WinPlace.Length);

            foreach (Tuple<Int32, Int32> a in e.WinPlace) //tényleg a nyertes karaktere van-e az átadott nyertes mezőkön
                Assert.AreEqual(e.WhoWon, _model.GetFieldValue(a.Item1, a.Item2));
        }

        #endregion
    }
}