using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Library.TorrentDownload
{
    public interface ITorrentDownloader
    {
        Task DownloadFile(string url,string basePath,string fileNameWithExtension);
        Task<string> GetHtmlContent(string url);
        void DecodeHtmlContent(Action<HtmlDocument, string> action, HtmlDocument document, string htmlContent);
    }
}
