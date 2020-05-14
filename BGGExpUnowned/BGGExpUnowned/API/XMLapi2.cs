using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    class XMLapi2 : IAPI
    {
        static readonly string BASE_URL = "https://boardgamegeek.com/";
        static readonly string API_URL = BASE_URL + "xmlapi2/";
        public List<BoardGame> GetCollectionWithoutExpansions(string username)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(API_URL + "collection?username=" + username + "&excludesubtype=boardgameexpansion&own=1"); //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&excludesubtype=boardgameexpansion&own=1
            XmlNodeList items = xDoc.GetElementsByTagName("item");
            List<BoardGame> collection = new List<BoardGame>();
            for(int i = 0; i < items.Count; i++)
            {
                XmlNode node = items[i];
                long id = Convert.ToInt64(node.Attributes["objectid"].Value);
                string name =  node.SelectSingleNode("name").InnerText;

                collection.Add(new BoardGame(id, name));
            }
            //TODO Add retries if status is 202. Add failure detection for everything else. 
            return collection;
        }

        public List<BoardGame> GetExpansionsOfGames(IEnumerable<long> IDs)
        {
            String ids = String.Join(",", IDs);

            //TODO test this with a big collection. Need to split up?

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(API_URL + "thing?id=" + ids);
            XmlNodeList links = xDoc.GetElementsByTagName("link");
            List<BoardGame> expansions = new List<BoardGame>();
            for (int i = 0; i < links.Count; i++)
            {
                XmlNode node = links[i];
                if (!node.Attributes["type"].Value.Equals("boardgameexpansion"))
                {
                    continue;
                }
                long id = Convert.ToInt64(node.Attributes["id"].Value);
                string name = node.Attributes["value"].Value;

                expansions.Add(new BoardGame(id, name));
            }
            return expansions;
        }
    }
}
