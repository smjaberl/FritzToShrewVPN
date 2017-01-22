/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FritzToShrewVPN
{
    class FritzConnector
    {

        avm theAVM;
        DataVPN theData;

        public FritzConnector(DataVPN data)
        {
            theData = data;
            theAVM = new avm(theData);
        }

        public void loadFromBox()
        {
            string sid = theAVM.GetSessionId();
            string[] Array;
            int c = 10;
            

            string patternUser = @"\[""boxusers:settings/user\[boxuser\d{2,}\]/name""\] = """;
            string patternPSK = @"\[""boxusers:settings/user\[boxuser\d{2,}\]/vpn_psk""\] = """;
            string patternHost = @"\[""jasonii:settings/dyndnsname""\] = """;
            do
            {
                Array = theAVM.GetPage(@"http://" + theData.LocalBox + "/system/vpn_print.lua?sid=" + sid + "&uid=boxuser"+c.ToString()).Split('\n');
                string user = "", psk = "", host = "";

                if (sid == "0000000000000000")
                {
                    MessageBox.Show(@"Ups, da gab es wohl ein Problem mit Passwort oder Benutzernamen.",
                                    @"Fehler",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error
                                   );
                    break;
                }


                foreach (string line in Array)
                {
                    if (Regex.Match(line,patternUser).Success)
                        user = Regex.Replace(Regex.Replace(line, patternUser, String.Empty), @""",*", String.Empty).Trim();
                    if (Regex.Match(line, patternPSK).Success)
                        psk = Regex.Replace(Regex.Replace(line, patternPSK, String.Empty), @""",*", String.Empty).Trim();
                    if (Regex.Match(line, patternHost).Success)
                        host = Regex.Replace(Regex.Replace(line, patternHost, String.Empty), @""",*", String.Empty).Trim();
                }
                if (psk == "nil")
                    break;
                theData.theUserDataObjects.Add(new UserDataObject(true, user, psk, user+"@fritz.box"));
                theData.Host = host;
                c++;
            } while (c<100);
        }
}
}