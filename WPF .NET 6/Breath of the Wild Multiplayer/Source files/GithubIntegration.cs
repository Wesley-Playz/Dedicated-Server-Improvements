using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Github
{
    public static class GithubIntegration
    {
        private static HttpClient client
        {
            get
            {
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("User-Agent", "request");
                //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", "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");
                return _client;
            }
        }

        public static async Task<string> Get(string Url)
        {
            return (await client.GetAsync(Url)).
                Content.ReadAsStringAsync().Result;
        }

        public static async Task<(string, string)> GetLatestVersion()
        {
            var ReleasesUrl = $"https://api.github.com/repos/edgarcantuco/BOTW.Release/releases";

            var Releases = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(await Get(ReleasesUrl));

            string LatestRelease = "0.0.0";
            string AssetsUrl = "";

            foreach(var release in Releases)
            {
                if (CompareVersion(LatestRelease, (string)release["tag_name"]))
                {
                    LatestRelease = (string)release["tag_name"];
                    AssetsUrl = (string)release["assets_url"];
                }
            }

            var Assets = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(await Get(AssetsUrl));

            return (LatestRelease, (string)Assets[0]["url"]);
        }

        public static bool CompareVersion(string oldVersion, string newVersion)
        {
            List<string> OldVersionNumber = oldVersion.Split(".").ToList();
            List<string> NewVersionNumber = newVersion.Split(".").ToList();

            for(int i = 0; i < 3; i++)
            {
                if (Int16.Parse(OldVersionNumber[i]) < Int16.Parse(NewVersionNumber[i]))
                    return true;

                if (Int16.Parse(OldVersionNumber[i]) > Int16.Parse(NewVersionNumber[i]))
                    return false;
            }

            return false;
        }

        public static async Task<string> DownloadZip(string Url)
        {
            string fileName = Guid.NewGuid().ToString();

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "request");
                //wc.Headers.Add(HttpRequestHeader.Authorization, "token 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");
                wc.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
                wc.DownloadFile(new System.Uri(Url), $"{fileName}.zip");
            }

            return fileName;
        }
    }
}
