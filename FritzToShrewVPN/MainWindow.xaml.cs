/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private FritzConnector theFritzConector = new FritzConnector(theDataVPN);

        public MainWindow()
        {
            InitializeComponent();
            foreach(var I in theShrewHandler.VPNs)
                comboBoxConnection.Items.Add(I);
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
            bool stdcon = Convert.ToBoolean(radioButtonStandard.IsChecked);

            if (theShrewHandler.make(stdcon))
                MessageBox.Show("Die VPN-Verbindung(en) wurde(n) erfolgreich erstellt.",
                            "Herzlichen Glückwunsch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                            );
            else
                MessageBox.Show("Die VPN-Verbindung(en) konnte(n) nicht erstellt werden. \nError Code: 42.001",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                            );
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible )
                theDataVPN.FBPasswd = passwordBox.Password;
            theFritzConector.loadFromBox();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void checkBoxUserLogin_Checked(object sender, RoutedEventArgs e)
        {
            checkBoxPasswordVisible.Content = "Benutzer Passwort sichtbar";
            textBoxUser.IsEnabled = true;
        }
        private void checkBoxUserLogin_UnChecked(object sender, RoutedEventArgs e)
        {
            checkBoxPasswordVisible.Content = "FritzBox Passwort sichtbar";
            textBoxUser.Text = "";
            textBoxUser.IsEnabled = false;
        }

        private void buttonLoadExistingCon_Click(object sender, RoutedEventArgs e)
        {
            theDataVPN.theUserDataObjects.Add(theShrewHandler.load());
        }

        private void comboBoxConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkBoxPasswordVisible_Checked(object sender, RoutedEventArgs e)
        {
            passwordBox.Visibility = Visibility.Hidden;
            textBoxPassword.Visibility = Visibility.Visible;
            textBoxPassword.Text = passwordBox.Password;
        }

        private void checkBoxPasswordVisible_UnChecked(object sender, RoutedEventArgs e)
        {
            textBoxPassword.Visibility = Visibility.Hidden;
            passwordBox.Visibility = Visibility.Visible; 
            passwordBox.Password = textBoxPassword.Text;
        }
    }
}
