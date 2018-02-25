using System.Collections.Generic;
using System.IO;

namespace Autoreplace
{
    /// <summary>
    /// Provides an API to operate PL/SQL Developer autoreplaces file like on usual list.
    /// </summary>
    public class AutoreplaceList
    {
        private LinkedList<AutoreplaceEntry> entries;

        /// <summary>
        /// Creates an empty instance of <see cref="AutoreplaceList"/>.
        /// </summary>
        public AutoreplaceList()
        {
            entries = new LinkedList<AutoreplaceEntry>();
        }

        /// <summary>
        /// Creates and populates an instance of <see cref="AutoreplaceList"/> from the specified PL/SQL Developer autoreplaces text file.
        /// </summary>
        /// <param name="filePath">Absolute path to the autoreplaces file.</param>
        /// <returns></returns>
        public static AutoreplaceList LoadFromFile(string filePath)
        {
            string line = null;
            string[] tokens;

            AutoreplaceList result = new AutoreplaceList();

            StreamReader reader = new StreamReader(filePath);
            AutoreplaceEntry e = null;

            while ((line = reader.ReadLine()) != null)
            {
                tokens = line.Split('=');
                e = new AutoreplaceEntry(tokens[0], tokens[1]);
                result.Entries.AddLast(e);
            }

            reader.Close();
            return result;
        }

        /// <summary>
        /// Entries, contained in the autoreplaces file.
        /// </summary>
        public LinkedList<AutoreplaceEntry> Entries
        {
            get
            {
                return entries;
            }
        }
    }
}
