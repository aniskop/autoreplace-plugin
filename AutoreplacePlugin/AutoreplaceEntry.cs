using System;

namespace Autoreplace
{
    /// <summary>
    /// Wrapper for autoreplaces file entry.
    /// </summary>
    public class AutoreplaceEntry
    {
        private const int PAD_LENGTH = 30;

        /// <summary>
        /// Creates and instance of <see cref="AutoreplaceEntry"/>.
        /// </summary>
        /// <param name="name">Alias given to the autoreplace command.</param>
        /// <param name="value">Autorepalce command.</param>
        public AutoreplaceEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Returns the alias, given to the autoreplace command.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the autoreplace command.
        /// </summary>
        public string Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks whether autoreplace entry contains given text.
        /// </summary>
        /// <param name="text">Text to search for.</param>
        /// <param name="compareOptions">Search options.</param>
        /// <returns><code>true</code> if <see cref="Name"/> or <see cref="Value"/> contains <paramref name="text"/>, <code>false</code> otherwise.</returns>
        public bool ContainsText(string text, StringComparison compareOptions)
        {
            return (Name.IndexOf(text, compareOptions) >= 0
                    || Value.IndexOf(text, compareOptions) >= 0);
        }

        /// <summary>
        /// Converts <see cref="AutoreplaceEntry"/> to a string representation.
        /// </summary>
        public override string ToString()
        {
            if (Name.Length < PAD_LENGTH)
            {
                return Name.PadRight(PAD_LENGTH) + " " + Value;
            } else
            {
                return Name + " " + Value;
            }
            
        }
    }
}
