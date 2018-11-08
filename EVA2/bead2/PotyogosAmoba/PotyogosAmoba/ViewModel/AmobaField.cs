using System;

namespace PotyogosAmoba.ViewModel
{
    public class AmobaField : ViewModelBase
    {
        private Boolean isClickable;
        private String _text;

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
        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
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
        public DelegateCommand StepCommand { get; set; }
    }
}
