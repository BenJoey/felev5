using System;

namespace PotyogosAmoba.ViewModel
{
    /// <summary>
    /// Potyogós Amőba játékmező típusa.
    /// </summary>
    public class AmobaField : ViewModelBase
    {
        private Boolean isClickable;
        private String _text;
        private Boolean winField;
        private Int32 _ButtonSize;

        /// <summary>
        /// Megjelenítendő betűméret lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 ButtonSize
        {
            get { return _ButtonSize; }
            set
            {
                if (_ButtonSize != value)
                {
                    _ButtonSize = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kattinthatóság lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean Clickable
        {
            get { return isClickable; }
            set
            {
                if(isClickable != value)
                {
                    isClickable = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Ez-e az egyik nyertes mező lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean isWinField
        {
            get { return winField; }
            set
            {
                if (winField != value)
                {
                    winField = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Felirat lekérdezése, vagy beállítása.
        /// </summary>
        public String Text
        {
            get { return _text; }
            set
            {
                OnPropertyChanged();
                if (_text != value)
                {
                    _text = value;
                }
            }
        }

        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Sorszám lekérdezése.
        /// </summary>
        public Int32 Number { get; set; }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }
    }
}
