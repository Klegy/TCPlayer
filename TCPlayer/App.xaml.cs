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
using System.Globalization;
using System.Resources;
using System.Windows;
using TCPlayer.Code;

namespace TCPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal const string Formats = "*.mp1;*.mp2;*.mp3;*.mp3pro;*.mp4;*.m4a;*.m4b;*.aac;*.flac;*.ac3;*.wv;*.wav;*.wma;*.asf;*.ogg;*.midi;*.mid;*.rmi;*.kar;*.xm;*.it;*.s3m;*.mod;*.mtm;*.umx;*.mo3;*.ape;*.mpc;*.mp+;*.mpp;*.ofr;*.ofs;*.spx;*.tta;*.dsf;*.dsdiff;*.opus";
        internal const string Playlists = "*.pls;*.m3u;*.wpl;*.asx";

        internal static Dictionary<string, string> CdData;
        internal static string DiscID;
        internal static HashSet<string> RecentUrls;

        internal static void SetAppCulture()
        {
            if (TCPlayer.Properties.Settings.Default.CultureOverride != null)
            {
                try
                {
                    CultureInfo @override = TCPlayer.Properties.Settings.Default.CultureOverride;
                    if (AvailableCultures.Contains(@override))
                        TCPlayer.Properties.Resources.Culture = @override;
                    else
                        TCPlayer.Properties.Settings.Default.CultureOverride = CultureInfo.InvariantCulture;
                }
                catch (Exception)
                {
                    TCPlayer.Properties.Settings.Default.CultureOverride = CultureInfo.InvariantCulture;
                }
            }
        }

        internal static void FillUrlList()
        {
            if (!TCPlayer.Properties.Settings.Default.RememberRecentURLs) return;
            var items = TCPlayer.Properties.Settings.Default.RecentURLs.Split('\n', '\r');
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item)) continue;
                RecentUrls.Add(item);
            }
        }

        public static void SaveRecentUrls()
        {
            if (!TCPlayer.Properties.Settings.Default.RememberRecentURLs) return;
            var sb = new System.Text.StringBuilder();
            foreach (var url in RecentUrls) sb.AppendLine(url);
            TCPlayer.Properties.Settings.Default.RecentURLs = sb.ToString();
        }

        public static List<CultureInfo> AvailableCultures
        {
            get
            {
                // Pass the class name of your resources as a parameter e.g. MyResources for MyResources.resx
                ResourceManager rm = new ResourceManager(typeof(Properties.Resources));

                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                List<CultureInfo> ret = new List<CultureInfo>(cultures.Length);
                foreach (CultureInfo culture in cultures)
                {
                    try
                    {
                        ResourceSet rs = rm.GetResourceSet(culture, true, false);
                        if (rs != null)
                        {
                            if (string.IsNullOrEmpty(culture.Name))
                                ret.Add(new CultureInfo("en-US"));
                            else
                                ret.Add(culture);
                        }
                    }
                    catch (CultureNotFoundException)
                    {
                    }
                }
                return ret;
            }
        }
    }
}
