using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Ink;
using System.Xml;
using com.mbpro.BGGExpUnowned.Extension;
using com.mbpro.BGGExpUnowned.model;
using Microsoft.VisualBasic;

namespace com.mbpro.BGGExpUnowned.API
{
    class XMLapi2 : IAPI
    {

        public List<BoardGame> GetCollectionOnlyExpansions(string username)
        {
            APICommand command = new CollectionCommand(username, false, true);//https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&subtype=boardgameexpansion&own=1
            return command.Execute();
        }

        public List<BoardGame> GetCollectionWithoutExpansions(string username)
        {
            //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&excludesubtype=boardgameexpansion&own=1
            APICommand command = new CollectionCommand(username, true, false);
            return command.Execute();
        }

        public List<BoardGame> GetExpansionsOfGames(IEnumerable<long> IDs)
        {
            APICommand command = new ThingCommand(IDs);
            return command.Execute();
        }

        
    }
}
