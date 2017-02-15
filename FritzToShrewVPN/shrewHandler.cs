/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows;


namespace FritzToShrewVPN
{
    class shrewHandler
    {

        DataVPN theData;
        Assembly _assembly;
        StreamReader _textStreamReader;

        public shrewHandler(DataVPN theDa)
        {
            theData = theDa;

            // read shrew config files
            if (Directory.Exists(ShrewSitesDir))
            {
                DirectoryInfo shrewDirectory = new System.IO.DirectoryInfo(ShrewSitesDir);
                foreach (System.IO.FileInfo f in shrewDirectory.GetFiles())
                {
                    VPNs.Add(f.Name);
                }
            }
        }

        public List<String> VPNs = new List<string>();

        // AppData directory of the current user
        private string ShrewSitesDir = Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\Shrew Soft VPN\sites";
        private string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public bool make(bool newFile)
        {

            string Config;
            string[] ConfArray;
            DateTime localDate = DateTime.Now;

            if (newFile)  // get default Config file which is an embeddet resource
            {
                _assembly = Assembly.GetExecutingAssembly();
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("FritzToShrewVPN.config.vpn"));
                Config = _textStreamReader.ReadToEnd();
            }
            else
            {
                Config = read();
            }

            ConfArray = Config.Split('\n');

            //replace operation in Config

            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            foreach (UserDataObject obj in theData.theUserDataObjects)
            {
                if (obj.Check)
                {
                    int i = 0;
                    foreach (string line in ConfArray)
                    {
                        if (line.StartsWith(@"s:network-host:"))
                            ConfArray[i] = @"s:network-host:" + theData.Host;
                        if (line.StartsWith(@"s:ident-client-data:"))
                            ConfArray[i] = @"s:ident-client-data:" + obj.User;
                        if (line.StartsWith(@"b:auth-mutual-psk:"))
                            ConfArray[i] = @"b:auth-mutual-psk:" + Base64Encode(obj.PSK);
                        i++;
                    }
                   

                    Config = string.Join("\n", ConfArray.ToArray());

                    //save vpn connection

                    bool cancel = false;

                    if (File.Exists(ShrewSitesDir + @"\" + obj.ConName))
                    {
                        var result = MessageBox.Show(ShrewSitesDir + @"\" + obj.ConName + " existiert schon, möchtest du die Datei überschreiben?",
                        @"Warnung",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question
                        );

                        switch (result)
                        {
                            case MessageBoxResult.Yes: break;
                            case MessageBoxResult.Cancel: cancel = true; break;    //Cancel all Operations
                            case MessageBoxResult.No: continue;       //Cancel Operations für one Connection
                        }
                        if (cancel) break;
                    }

                    try
                    {
                        File.WriteAllText(ShrewSitesDir + @"\" + obj.ConName, Config);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(),
                        @"Fehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
                        return false;
                    }



                    // wirte the shortcut to the deskop
                    if (theData.ShortCut)
                    {
                        string cmd = "\"" + @"C:\Program Files\ShrewSoft\VPN Client\" + "ipsecc.exe" + "\"" + " -r " + obj.ConName + " -u " + obj.User;

                        try
                        {
                            File.WriteAllText(DesktopPath + @"\" + obj.ConName + ".cmd", cmd);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(),
                            @"Fehler",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                            );
                            return false;
                        }
                    }
                }
                
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------
            return true;
        }


        // load the config file and write the data to theData
        public UserDataObject load()
        {
            string Config = read();
            string[] ConfArray;
            string u = "", p = "", host = "";

            ConfArray = Config.Split('\n');
            int i = 0;
            foreach (string line in ConfArray)
            {
                if (line.StartsWith(@"s:network-host:"))
                    host = ConfArray[i].Substring("s:network-host:".Count()).Trim();
                if (line.StartsWith(@"s:ident-client-data:"))
                    u = ConfArray[i].Substring(@"s:ident-client-data:".Count()).Trim();
                if (line.StartsWith(@"b:auth-mutual-psk:"))
                    p = Base64Decode(ConfArray[i].Substring(@"b:auth-mutual-psk:".Count()));
                i++;
            }

            theData.Host = host;
            return new UserDataObject(true, u, p, theData.SelectedCon);
        }

        private string read()
        {
            string Config = "";
            try
            {
                Config = File.ReadAllText(ShrewSitesDir + @"\" + theData.SelectedCon, Encoding.Default);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(@"Fehler",
                @"Lesen von " + ShrewSitesDir + @"\" + theData.SelectedCon + @" war nicht möglich" + "Error: 42.002",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
                Config = "";
            }
            return Config;
        }

        private string Base64Encode(string plainText)
        {
            if (plainText == null)
                plainText = "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}