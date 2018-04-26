using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadLib
{
    public enum Restrictions
    {
        NoRestriction,
        InsideCurrentDomain,
        NotUpperPathSourcUri
    }

    public class DownloaderSettings
    {
        public string Path { get; set; }
        public int Deep { get; set; }
        public Restrictions Restriction { get; set; }
        public IEnumerable<string> AllowableExtensions { get; set; }
        public bool DoTrace { get; set; }

    }
}
