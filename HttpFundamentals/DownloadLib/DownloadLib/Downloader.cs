using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;

namespace DownloadLib
{
    public class Downloader
    {
        private DownloaderSettings settings;

        private readonly int levelCounter;
        private int currentLevel;
        private readonly string rootPath;
        private Dictionary<string, string> externalLinks;
        private string domain;

        public Downloader(DownloaderSettings settings)
        {
            this.settings = settings;

            rootPath = settings.Path;
            levelCounter = settings.Deep;
            currentLevel = 0;
            externalLinks = new Dictionary<string, string>();
        }

        public string DownloadSite(string fullFilePath, string uri, bool createSubFolders = true)
        {
            if (domain == null)
                domain = GetDomain(uri);

            var filePath = string.Empty;

            currentLevel++;

            var file = File.Create(fullFilePath);
            file.Dispose();
            filePath = fullFilePath;

            if (createSubFolders)
            {
                var imgFolder = Directory.CreateDirectory(rootPath + "images");
                var scrFolder = Directory.CreateDirectory(rootPath + "scripts");
                var linksFolder = Directory.CreateDirectory(rootPath + "links");
            }

            var result = string.Empty;
            try
            {
                result = DownloadPageNow(uri).Result;
            }
            catch (Exception ex)
            {

            }

            var parser = new HtmlParser();
            var doc = parser.Parse(result);

            var images = doc.QuerySelectorAll("img");

            if (images.Count() > 0)
                DownloadLinks(uri, rootPath, "images", images, ref result);

            var scripts = doc.QuerySelectorAll("script");

            if (scripts.Count() > 0)
                DownloadLinks(uri, rootPath, "scripts", scripts, ref result);

            var links = doc.QuerySelectorAll("link");

            if (links.Count() > 0)
                DownloadLinks(uri, rootPath, "links", links, ref result);

            var refs = doc.GetElementsByTagName("a");

            if (refs.Count() > 0 && currentLevel < levelCounter)
            {
                foreach (var refer in refs)
                {
                    var link = refer.GetAttribute("href");
                    if (!string.IsNullOrEmpty(link) && link != "/")
                    {
                        var reference = GetWorkingLink(link, uri);
                        if (CheckRestriction(reference, settings.Restriction, domain))
                        {
                            if (externalLinks.ContainsKey(link))
                            {
                                result = result.Replace(link, externalLinks[link]);
                            }
                            else
                            {
                                var name = Guid.NewGuid() + ".html";
                                var newFile = File.Create(rootPath + name);
                                newFile.Dispose();
                                var newFilePath = rootPath + name;

                                externalLinks.Add(link, newFilePath);
                                DownloadSite(newFilePath, reference, false);
                                result = result.Replace("\"" + link + "\"", "\"" + newFilePath + "\"");
                            }
                        }
                    }
                }
            }

            File.WriteAllText(filePath, result);

            currentLevel--;

            return "Downloaded " + filePath; ;
        }

        private async Task<string> DownloadPageNow(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    using (var content = response.Content)
                    {
                        var str = await content.ReadAsStringAsync();
                        return str;
                    }
                }
            }
        }

        private void DownloadLinks(string uri, string folderPath, string subFolder, IHtmlCollection<IElement> links, ref string page, string extension = "")
        {
            foreach (var link in links)
            {
                var src = link.GetAttribute("src");
                if (string.IsNullOrEmpty(src))
                    src = link.GetAttribute("href");
                if (string.IsNullOrEmpty(src))
                    continue;

                var ext = src.Substring(src.Count() - 5, 5);
                bool allowed = false;

                if (settings.AllowableExtensions == null || settings.AllowableExtensions.Count() == 0)
                {
                    allowed = true;
                }
                else
                {
                    foreach (var exten in settings.AllowableExtensions)
                    {
                        if (ext.Contains(exten))
                            allowed = true;
                    }
                }

                if (!allowed)
                    continue;

                var src2 = GetWorkingLink(src, uri);

                var imageArray = new byte[] { };
                try
                {
                    imageArray = DownloadData(src2).Result;
                }
                catch
                {
                    return;
                }
                var name = string.Empty;
                if (string.IsNullOrEmpty(extension))
                    name = Guid.NewGuid().ToString();
                else
                    name = Guid.NewGuid().ToString() + extension;

                if (src.Contains(".css"))
                    name = Guid.NewGuid().ToString() + ".css";

                if (src.Contains(".js"))
                    name = Guid.NewGuid().ToString() + ".js";

                File.WriteAllBytes(folderPath + subFolder + @"\\" + name, imageArray);

                page = page.Replace(src, folderPath + subFolder + @"\" + name);
            }
        }

        private async Task<byte[]> DownloadData(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return new byte[1];

                    using (var content = response.Content)
                    {
                        var str = await content.ReadAsByteArrayAsync();
                        return str;
                    }
                }
            }
        }

        private string GetProtocol(string uri)
        {
            if (uri.Contains("https"))
                return "https:";
            else
                return "http:";
        }

        private string GetWorkingLink(string link, string uri)
        {
            if (string.IsNullOrEmpty(link) || link.Count() == 1)
                return rootPath + "index.html";

            var result = string.Empty;
            if (link.First() != 'h')
                result = uri + link;
            if (link.First() == '/' && link.Skip(1).First() == '/')
                result = GetProtocol(uri) + link;
            if (string.IsNullOrEmpty(result))
                result = link;

            return result;
        }

        private string GetDomain(string url)
        {
            return url.Split('.').First();
        }

        private bool CheckRestriction(string url, Restrictions restriction, string domain)
        {
            if (restriction == Restrictions.NoRestriction)
                return true;
            if (restriction == Restrictions.InsideCurrentDomain)
                return url.Contains(domain) ? true : false;

            throw new NotSupportedException();
        }
    }
}
