using DownloadLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloadLibUser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\MentoringGit\mentor7-16\local\";
            var settings = new DownloaderSettings();
            settings.Path = path;
            settings.Deep = 3;
            settings.DoTrace = true;
            settings.Restriction = Restrictions.InsideCurrentDomain;
            //settings.AllowableExtensions = new List<string>() {".gif", ".png", ".js", ".css"};
            var loader = new Downloader(settings);
             var result = loader.DownloadSite(path + "index.html", "http://juniup.ru/");

            Console.WriteLine(result);
            Console.ReadKey();
        }
    };
}
