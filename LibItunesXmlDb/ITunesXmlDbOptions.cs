using System;
using System.Collections.Generic;

namespace Webmaster442.LibItunesXmlDb
{
    /// <summary>
    /// A class representing various parser options
    /// </summary>
    public sealed class ITunesXmlDbOptions : IEquatable<ITunesXmlDbOptions?>
    {
        /// <summary>
        /// Exclude tracks that don't exist on the user's system
        /// Default value is false
        /// </summary>
        public bool ExcludeNonExistingFiles { get; set; }

        /// <summary>
        /// Enable or Disable paralel track parsing.
        /// Default value is true
        /// </summary>
        public bool ParalelParsingEnabled { get; set; }

        /// <summary>
        /// Creates a new instance of ITunesXmlDbOptions
        /// </summary>
        public ITunesXmlDbOptions()
        {
            ParalelParsingEnabled = true;
            ExcludeNonExistingFiles = false;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ITunesXmlDbOptions);
        }

        public bool Equals(ITunesXmlDbOptions? other)
        {
            return other != null &&
                   ExcludeNonExistingFiles == other.ExcludeNonExistingFiles &&
                   ParalelParsingEnabled == other.ParalelParsingEnabled;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExcludeNonExistingFiles, ParalelParsingEnabled);
        }

        public static bool operator ==(ITunesXmlDbOptions? left, ITunesXmlDbOptions? right)
        {
            return EqualityComparer<ITunesXmlDbOptions?>.Default.Equals(left, right);
        }

        public static bool operator !=(ITunesXmlDbOptions? left, ITunesXmlDbOptions? right)
        {
            return !(left == right);
        }
    }
}
