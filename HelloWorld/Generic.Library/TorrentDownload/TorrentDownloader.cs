using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Library.TorrentDownload
{
    public class TorrentDownloader : ITorrentDownloader
    {
        public void DecodeHtmlContent(Action<HtmlDocument, string> action, HtmlDocument document, string htmlContent)
        {
            action?.Invoke(document, htmlContent);
        }

        public async Task DownloadFile(string url, string basePath, string fileNameWithExtension)
        {
            string u = url;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(u, HttpCompletionOption.ResponseContentRead);
                var htmlContent = await response.Content.ReadAsByteArrayAsync();
                string filePath = Path.Combine(basePath, fileNameWithExtension);
                await File.WriteAllBytesAsync(filePath, htmlContent);
            }
        }

        public async Task<string> GetHtmlContent(string url)
        {
            string u = url;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(u, HttpCompletionOption.ResponseContentRead);
                var htmlContent = await response.Content.ReadAsStringAsync();
                return htmlContent;
            }
        }
    }
}
