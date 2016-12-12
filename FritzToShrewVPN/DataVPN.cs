/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzToShrewVPN
{
    class DataVPN : INotifyPropertyChanged
    {
        public DataVPN()
        {
            conName = "FritzBox";
            shortCut = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string argument)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(argument));
            }
        }


        private string conName;
        public string ConName
        {
            get { return conName; }
            set
            {
                conName = value;
                Notify("conName");
            }
        }

        private string host;
        public string Host
        {
            get { return host; }
            set
            {
                host = value;
                Notify("host");
            }
        }

        private string user;
        public string User
        {
            get { return user; }
            set
            {
                user = value;
                Notify("user");
            }
        }

        private string psk;
        public string PSK
        {
            get { return psk; }
            set
            {
                psk = value;
                Notify("psk");
            }
        }
        private bool newCon;
        public bool NewCon
        {
            get { return newCon; }
            set
            {
                newCon = value;
                Notify("newCon");
            }
        }
        private bool shortCut;
        public bool ShortCut
        {
            get { return shortCut; }
            set
            {
                shortCut = value;
                Notify("shortCut");
            }
        }
    }
}
