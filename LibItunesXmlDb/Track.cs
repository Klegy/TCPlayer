using System;
using System.Collections.Generic;

namespace Webmaster442.LibItunesXmlDb
{
    /// <summary>
    /// A class representing tracks in the iTunes Database
    /// </summary>
    public sealed class Track : IEquatable<Track?>
    {
        /// <summary>
        /// Track Id
        /// </summary>
        public int TrackId { get; set; }
        /// <summary>
        /// Track Title
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Track Artist
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// Track Album Artist
        /// </summary>
        public string AlbumArtist { get; set; }
        /// <summary>
        /// Track Composer
        /// </summary>
        public string Composer { get; set; }
        /// <summary>
        /// Track Album
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// Track Genre
        /// </summary>
        public string Genre { get; set; }
        /// <summary>
        /// Track Kind
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// Track size in bytes
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Track length
        /// </summary>
        public string PlayingTime { get; set; }
        /// <summary>
        /// Track number
        /// </summary>
        public int? TrackNumber { get; set; }
        /// <summary>
        /// Track year
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// Last modification date
        /// </summary>
        public DateTime? DateModified { get; set; }
        /// <summary>
        /// Date added
        /// </summary>
        public DateTime? DateAdded { get; set; }
        /// <summary>
        /// Track bitrate
        /// </summary>
        public int? BitRate { get; set; }
        /// <summary>
        /// Track sample rate
        /// </summary>
        public int? SampleRate { get; set; }
        /// <summary>
        /// Play count
        /// </summary>
        public int? PlayCount { get; set; }
        /// <summary>
        /// Last play date
        /// </summary>
        public DateTime? PlayDate { get; set; }
        /// <summary>
        /// Part of compilation flag
        /// </summary>
        public bool PartOfCompilation { get; set; }
        /// <summary>
        /// File Path
        /// </summary>
        public string FilePath { get; set; }

        public Track()
        {
            FilePath = string.Empty;
            Name = string.Empty;
            Artist = string.Empty;
            AlbumArtist = string.Empty;
            Composer = string.Empty;
            Album = string.Empty;
            Genre = string.Empty;
            Kind = string.Empty;
            PlayingTime = string.Empty;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Track);
        }

        public bool Equals(Track? other)
        {
            return other != null &&
                   TrackId == other.TrackId &&
                   Name == other.Name &&
                   Artist == other.Artist &&
                   AlbumArtist == other.AlbumArtist &&
                   Composer == other.Composer &&
                   Album == other.Album &&
                   Genre == other.Genre &&
                   Kind == other.Kind &&
                   Size == other.Size &&
                   PlayingTime == other.PlayingTime &&
                   TrackNumber == other.TrackNumber &&
                   Year == other.Year &&
                   DateModified == other.DateModified &&
                   DateAdded == other.DateAdded &&
                   BitRate == other.BitRate &&
                   SampleRate == other.SampleRate &&
                   PlayCount == other.PlayCount &&
                   PlayDate == other.PlayDate &&
                   PartOfCompilation == other.PartOfCompilation &&
                   FilePath == other.FilePath;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(TrackId);
            hash.Add(Name);
            hash.Add(Artist);
            hash.Add(AlbumArtist);
            hash.Add(Composer);
            hash.Add(Album);
            hash.Add(Genre);
            hash.Add(Kind);
            hash.Add(Size);
            hash.Add(PlayingTime);
            hash.Add(TrackNumber);
            hash.Add(Year);
            hash.Add(DateModified);
            hash.Add(DateAdded);
            hash.Add(BitRate);
            hash.Add(SampleRate);
            hash.Add(PlayCount);
            hash.Add(PlayDate);
            hash.Add(PartOfCompilation);
            hash.Add(FilePath);
            return hash.ToHashCode();
        }

        public static bool operator ==(Track? left, Track? right)
        {
            return EqualityComparer<Track?>.Default.Equals(left, right);
        }

        public static bool operator !=(Track? left, Track? right)
        {
            return !(left == right);
        }
    }
}
