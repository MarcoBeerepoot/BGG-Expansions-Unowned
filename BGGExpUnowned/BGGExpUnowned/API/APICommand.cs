using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    abstract class APICommand
    {
        public static readonly string BASE_URL = "https://boardgamegeek.com/";
        public static readonly string API_URL = BASE_URL + "xmlapi2/";
        private static readonly int MAX_ATTEMPTS = 5;
        private static readonly int TIME_BETWEEN_ATTEMPTS = 3000;

        protected XmlDocument XDoc = new XmlDocument();
        protected abstract void Process();
        protected List<BoardGame> Result = new List<BoardGame>();
        internal abstract string CreateURL();
        public virtual async Task<List<BoardGame>> ExecuteAsync()
        {
            await LoadStreamFromUrlWithRetriesAsync(CreateURL());
            Process();
            return Result;
        }

        internal async Task LoadStreamFromUrlWithRetriesAsync(string url)
        {
            for(int attempts = 0; attempts < MAX_ATTEMPTS; attempts++)
            {
                bool success = await LoadStreamAsync(url);
                if (success)
                {
                    return;
                }
                await Task.Delay(TIME_BETWEEN_ATTEMPTS);
            }
        }

        private async Task<bool> LoadStreamAsync(string url)
        {
            bool success = false;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode.Equals(HttpStatusCode.Accepted))
                {
                    return success;
                } else if(!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    throw new BGGAPIException("Request to BGG XML API failed with statuscode " + response.StatusCode + ".");
                }

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    XDoc.Load(stream);
                    success = true;
                }
            }
            return success;
        }
        
    }
}
