using System;

namespace ZH.ViewModel
{
    /// <summary>
    /// Potyogós Amőba játékmező típusa.
    /// </summary>
    public class GameField : ViewModelBase
    {
        private Boolean isClickable;
        private String _text;
        private String _col;


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

        public String Color
        {
            get { return _col; }
            set
            {
                OnPropertyChanged();
                if (_col != value)
                {
                    _col = value;
                }
            }
        }

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
