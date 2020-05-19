using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Ink;
using System.Xml;
using com.mbpro.BGGExpUnowned.Extension;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    class XMLapi2 : IAPI
    {
        static readonly string BASE_URL = "https://boardgamegeek.com/";
        static readonly string API_URL = BASE_URL + "xmlapi2/";
        static readonly int MAX_GAMES_BATCH_SIZE = 200; //Based on an id of max 6 digits for a board game + a comma to seperate I think 200 is a pretty safe bet. 

        public List<BoardGame> GetCollectionOnlyExpansions(string username)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(API_URL + "collection?username=" + username + "&subtype=boardgameexpansion&own=1"); //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&subtype=boardgameexpansion&own=1

            return ProcessCollection(xDoc);
        }

        public List<BoardGame> GetCollectionWithoutExpansions(string username)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(API_URL + "collection?username=" + username + "&excludesubtype=boardgameexpansion&own=1"); //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&excludesubtype=boardgameexpansion&own=1
            
            return ProcessCollection(xDoc);
        }

        private List<BoardGame> ProcessCollection(XmlDocument xDoc)
        {
            //TODO Add retries if status is 202. Add failure detection for everything else. 
            XmlNodeList items = xDoc.GetElementsByTagName("item");
            List<BoardGame> collection = new List<BoardGame>();
            for (int i = 0; i < items.Count; i++)
            {
                XmlNode node = items[i];
                long id = Convert.ToInt64(node.Attributes["objectid"].Value);
                string name = node.SelectSingleNode("name").InnerText;

                collection.Add(new BoardGame(id, name));
            }
            return collection;
        }

        public List<BoardGame> GetExpansionsOfGames(IEnumerable<long> IDs)
        {
            List<BoardGame> expansions = new List<BoardGame>();
            foreach (var Batch in IDs.Batch(MAX_GAMES_BATCH_SIZE))
            {
                String ids = String.Join(",", Batch);
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(API_URL + "thing?id=" + ids);
                XmlNodeList links = xDoc.GetElementsByTagName("link");
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
            }
            
            return expansions;
        }

        
    }
}
