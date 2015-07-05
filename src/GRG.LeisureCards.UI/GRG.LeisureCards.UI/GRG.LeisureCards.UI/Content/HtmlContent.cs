using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using log4net;

namespace GRG.LeisureCards.UI.Content
{
    public static class HtmlContent
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        private static readonly IDictionary<string, IDictionary<string, IDictionary<string, string>>> ViewContent = new Dictionary<string,IDictionary<string, IDictionary<string, string>>>();
        private static readonly IDictionary<string, string> Empty = new Dictionary<string, string>();

        public static void Init(HttpServerUtility server, string[] tenantKeys)
        {
            foreach (var tenantKey in tenantKeys)
            {
                ViewContent.Add(tenantKey, new Dictionary<string, IDictionary<string, string>>());
                try
                {
                    foreach (var dirInfo in Directory.EnumerateDirectories(server.MapPath(string.Format("~/Content/{0}/html", tenantKey))).Select(dir => new DirectoryInfo(dir)))
                    {
                        ViewContent[tenantKey].Add(dirInfo.Name, new Dictionary<string, string>());
                        
                        foreach (var file in dirInfo.EnumerateFiles("*.html"))
                            ViewContent[tenantKey][dirInfo.Name].Add(file.Name.Substring(0, file.Name.Length - 5), File.ReadAllText(file.FullName));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        public static IDictionary<string, string> GetContent(string tenantKey, string key)
        {
            return (ViewContent.ContainsKey(tenantKey) && ViewContent[tenantKey].ContainsKey(key)) ?
                ViewContent[tenantKey][key] :
                Empty;
        }
    }
}