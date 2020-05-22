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
            //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&subtype=boardgameexpansion&own=1
            APICommand command = new Builder<CollectionCommand>().With(c => c.Username = username).With(c => c.OnlyExpansions = true).Build();
            return command.Execute();
        }

        public List<BoardGame> GetCollectionWithoutExpansions(string username)
        {
            //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&excludesubtype=boardgameexpansion&own=1
            APICommand command = new Builder<CollectionCommand>().With(c => c.Username = username).With(c => c.ExcludeExpansions = true).Build();
            return command.Execute();
        }

        public List<BoardGame> GetExpansionsOfGames(IEnumerable<long> IDs)
        {
            APICommand command = new Builder<ThingCommand>().With(c => c.IDs = IDs).Build();
            return command.Execute();
        }

        
    }
}
