using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{

    /**
     * Command to query anything on the /collection path of the api.
     * Yes this is far from everything you can do with it, but it is a very large API, so I prefer to change things as I go.
     */
    class CollectionCommand : APICommand
    {
        public string Username { get; set; }
        public bool ExcludeExpansions { get; set; } = false;
        public bool OnlyExpansions { get; set; } = false;

        protected override bool Load()
        {
            XDoc.Load(CreateURL()); 
            return true;
        }

        protected override void Process()
        {
            //TODO add retries on everything other than 202.
            XmlNodeList items = XDoc.GetElementsByTagName("item");
            for (int i = 0; i < items.Count; i++)
            {
                XmlNode node = items[i];
                long id = Convert.ToInt64(node.Attributes["objectid"].Value);
                string name = node.SelectSingleNode("name").InnerText;

                Result.Add(new BoardGame(id, name));
            }
        }

        private string CreateURL()
        {
            StringBuilder sb = new StringBuilder(API_URL);
            sb.Append("collection?username=");
            sb.Append(Username);
            if (ExcludeExpansions)
            {
                sb.Append("&excludesubtype=boardgameexpansion");
            } else if (OnlyExpansions)
            {
                sb.Append("&subtype=boardgameexpansion");
            }
            sb.Append("&own=1");
            return sb.ToString();
        }
    }
}
