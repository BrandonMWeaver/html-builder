using HTMLBuilderUI.Models.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HTMLBuilderUI.ViewModels.DocumentViewModelControls
{
    class SelectPreviousElementControl : NotificationBase
    {
        private string _buttonMargin;

        public string ButtonMargin
        {
            get { return this._buttonMargin; }
            set
            {
                this._buttonMargin = value;
                this.OnPropertyChanged(nameof(this.ButtonMargin));
            }
        }

        private int _buttonHeight;

        public int ButtonHeight
        {
            get { return this._buttonHeight; }
            set
            {
                this._buttonHeight = value;
                this.OnPropertyChanged(nameof(this.ButtonHeight));
            }
        }

        private Visibility _buttonVisibility;

        public Visibility ButtonVisibility
        {
            get { return this._buttonVisibility; }
            set
            {
                this._buttonVisibility = value;
                this.OnPropertyChanged(nameof(this.ButtonVisibility));
            }
        }

        private bool _isAvailable;

        public bool IsAvailable
        {
            get { return this._isAvailable; }
            set
            {
                this._isAvailable = value;
                this.ButtonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                this.ButtonMargin = value ? "0,10,0,0" : "0,5,0,0";
                this.ButtonHeight = value ? 25 : 0;
                this.OnPropertyChanged(nameof(this.IsAvailable));
            }
        }
    }
}
