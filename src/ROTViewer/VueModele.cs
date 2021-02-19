using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Princeps.Framework.Interop;

namespace ROTViewer
{
    public class VueModele : INotifyPropertyChanged
    {
        private List<string> _monikers;

        public List<string> Monikers
        {
            get
            {
                return _monikers;
            }
            set
            {
                _monikers = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Monikers"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshTable()
        {
            Monikers=ROTable.Monikers.Select(x => x.Name).ToList();
        }

    }
}
