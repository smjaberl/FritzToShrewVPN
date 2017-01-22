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
    class UserDataObject : INotifyPropertyChanged
    {
        public UserDataObject() { }

        public UserDataObject(bool ch, string u, string p, string con)
        {
            check = ch;
            user = u;
            psk = p;
            conName = con;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string argument)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(argument));
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

        private bool check;
        public bool Check
        {
            get { return check; }
            set
            {
                check = value;
                Notify("check");
            }
        }

    }
}
