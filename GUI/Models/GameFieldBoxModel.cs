using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI.Models
{
    public class GameFieldBoxModel : PropertyChangedBase
    {
        private int _row;
        private int _column;
        private int _owner;
        private bool _hasStone;
        private Brush _stoneColor;

        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                NotifyOfPropertyChange();
            }
        }

        public int Column 
        { 
            get => _column; 
            set
            {
                _column = value;
                NotifyOfPropertyChange();
            }
        }

        public int Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                NotifyOfPropertyChange();
            }
        }

        public bool HasStone 
        { 
            get => _hasStone;
            set
            {
                _hasStone = value;
                NotifyOfPropertyChange();
            } 
        }

        public Brush StoneColor
        {
            get => _stoneColor;
            set 
            { 
                _stoneColor = value;
                NotifyOfPropertyChange();
            }
        }

    }
}
