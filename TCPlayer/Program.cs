/*
    TC Plyer
    Total Commander Audio Player plugin & standalone player written in C#, based on bass.dll components
    Copyright (C) 2016 Webmaster442 aka. Ruzsinszki Gábor

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Windows;
using TCPlayer.Code;

namespace TCPlayer
{
    public static class Program
    {
        private static bool _active;
        private static bool _prevactive;

        private const string AppName = "TCPlayer";

        [STAThread]
        public static void Main()
        {
            var si = new SingleInstanceApp(AppName);
            si.ReceiveString += Si_ReceiveString;
            if (si.IsFirstInstance)
            {
                var hasher = new EngineHashChecker();
                if (!hasher.CheckHashes())
                {
                    MessageBox.Show(TCPlayer.Properties.Resources.Error_CorruptDll,
                                    TCPlayer.Properties.Resources.Error_Title,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                App.SetAppCulture();
                var application = new App();
                App.CdData = new Dictionary<string, string>();
                App.DiscID = "";
                App.RecentUrls = new HashSet<string>();
                App.FillUrlList();
                application.InitializeComponent();
                _prevactive = true;
                _active = true;
                application.ShutdownMode = ShutdownMode.OnMainWindowClose;
                application.MainWindow = new MainWindow();
                application.MainWindow.Activated += MainWindow_Activated;
                application.MainWindow.Deactivated += MainWindow_Deactivated;
                application.Run(application.MainWindow);
                si.Close();
            }
            else si.SubmitParameters();
        }

        private static void Si_ReceiveString(string obj)
        {
            var files = obj.Split('\n');
            App.Current.Dispatcher.Invoke(() =>
            {
                var mw = App.Current.MainWindow as MainWindow;
                mw.DoLoadAndPlay(files);
            });
        }


        private static void MainWindow_Deactivated(object sender, EventArgs e)
        {
            _prevactive = _active;
            _active = false;
        }

        private static void MainWindow_Activated(object sender, EventArgs e)
        {
            _prevactive = _active;
            _active = true;
        }

        public static bool WasActivated
        {
            get
            {
                bool decision = _prevactive == false && _active == true;
                if (decision)
                {
                    _prevactive = true;
                }
                return decision;
            }
        }
    }
}
