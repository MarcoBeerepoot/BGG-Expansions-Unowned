﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using com.mbpro.BGGExpUnowned.Extension;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{

    /**
     * Command to query anything on the /thing path of the api.
     * Yes this is far from everything you can do with it, but it is a very large API, so I prefer to change things as I go.
     */
    class ThingCommand : APICommand
    {

        private static readonly int MAX_GAMES_BATCH_SIZE = 200; //Based on an id of max 6 digits for a board game + a comma to seperate I think 200 is a pretty safe bet. 
        public IEnumerable<long> IDs { get; set; }
        private IEnumerable<long> CurrentBatch;

        public async override Task<List<BoardGame>> ExecuteAsync()
        {
            foreach (var Batch in IDs.Batch(MAX_GAMES_BATCH_SIZE))
            {
                CurrentBatch = Batch;
                XDoc = new XmlDocument();
                await LoadStreamFromUrlWithRetriesAsync(CreateURL());
                Process();
            }

            return Result;
        }

        protected override void Process()
        {
            XmlNodeList links = XDoc.GetElementsByTagName("link");
            for (int i = 0; i < links.Count; i++)
            {
                XmlNode node = links[i];
                if (!node.Attributes["type"].Value.Equals("boardgameexpansion"))
                {
                    continue;
                }
                long id = Convert.ToInt64(node.Attributes["id"].Value);
                string name = node.Attributes["value"].Value;

                Result.Add(new BoardGame(id, name));
            }
        }

        internal override string CreateURL()
        {
            String ids = String.Join(",", CurrentBatch);
            return API_URL + "thing?id=" + ids;
        }
    }
}
