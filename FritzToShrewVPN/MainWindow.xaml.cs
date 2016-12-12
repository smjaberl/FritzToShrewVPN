/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FritzToShrewVPN
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private static DataVPN theDataVPN = new DataVPN();
        private shrewHandler theShrewHandler = new shrewHandler(theDataVPN);

        public MainWindow()
        {
            InitializeComponent();
            foreach(var I in theShrewHandler.VPNs)
                comboBoxConnection.Items.Add(I);
            comboBoxConnection.SelectedIndex = 0;
            if (comboBoxConnection.Items.Count == 0)
            {
                radioButtonCopy.IsEnabled = false;
                radioButtonEdit.IsEnabled = false;
            }
            
            
            this.DataContext = theDataVPN;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void buttonSaveExit_Click(object sender, RoutedEventArgs e)
        {
            Save();
            this.Close();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save()
        {
            bool newcon = Convert.ToBoolean(radioButtonNew.IsChecked);
            bool copy = Convert.ToBoolean(radioButtonCopy.IsChecked);


            if (theShrewHandler.make(newcon, copy))
                MessageBox.Show("Die VPN-Verbindung " + theDataVPN.ConName + " wurde erfolgreich erstellt.",
                            "Herzlichen Glückwunsch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                            );
            else
                MessageBox.Show("Die VPN-Verbindung " + theDataVPN.ConName + " konnte nicht erstellt werden. \nError Code: 42.001",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                            );

        }

        private void comboBoxConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxConnection.IsEnabled)
            {
                theDataVPN.ConName = comboBoxConnection.SelectedItem.ToString();
                theShrewHandler.load();
                if (Convert.ToBoolean(radioButtonCopy.IsChecked))
                    theDataVPN.ConName += @" - Kopie";
            }
        }

        private void radioButtonNew_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxConnection.IsEnabled = false;
            comboBoxConnection.Text = "";
            textBoxConnection.IsEnabled = true;
            theDataVPN.ConName = "FritzBox";
            theDataVPN.Host = "";
            theDataVPN.User = "";
            theDataVPN.PSK = "";
        }

        private void radioButtonEdit_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxConnection.IsEnabled = true;
            textBoxConnection.IsEnabled = false;
        }

        private void radioButtonCopy_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxConnection.IsEnabled = true;
            textBoxConnection.IsEnabled = true;
            theDataVPN.ConName += @" - Kopie";
        }
    }
}
