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
using System.Web;

namespace TCPlayer.Controls.Network
{
    public sealed class NetworkSearchProvider : IEquatable<NetworkSearchProvider?>
    {
        public string? Name { get; set; }
        public string? UriTemplate { get; set; }

        public string? GetFullUri(string? parameter)
        {
            if (string.IsNullOrEmpty(UriTemplate)) return null;
            return string.Format(UriTemplate, HttpUtility.UrlEncode(parameter));
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as NetworkSearchProvider);
        }

        public bool Equals(NetworkSearchProvider? other)
        {
            return other != null &&
                   Name == other.Name &&
                   UriTemplate == other.UriTemplate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, UriTemplate);
        }

        public static bool operator ==(NetworkSearchProvider? left, NetworkSearchProvider? right)
        {
            return EqualityComparer<NetworkSearchProvider>.Default.Equals(left, right);
        }

        public static bool operator !=(NetworkSearchProvider? left, NetworkSearchProvider? right)
        {
            return !(left == right);
        }
    }
}
