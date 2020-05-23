using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    class XMLapi2 : IAPI
    {

        public async Task<List<BoardGame>> GetCollectionOnlyExpansionsAsync(string username)
        {
            //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&subtype=boardgameexpansion&own=1
            APICommand command = new Builder<CollectionCommand>().With(c => c.Username = username).With(c => c.OnlyExpansions = true).Build();
            return await command.ExecuteAsync();
        }

        public async Task<List<BoardGame>> GetCollectionWithoutExpansionsAsync(string username)
        {
            //https://boardgamegeek.com/xmlapi2/collection?username=DraedGhawl&excludesubtype=boardgameexpansion&own=1
            APICommand command = new Builder<CollectionCommand>().With(c => c.Username = username).With(c => c.ExcludeExpansions = true).Build();
            return await command.ExecuteAsync();
        }

        public async Task<List<BoardGame>> GetExpansionsOfGamesAsync(IEnumerable<long> IDs)
        {
            APICommand command = new Builder<ThingCommand>().With(c => c.IDs = IDs).Build();
            return await command.ExecuteAsync();
        }

        
    }
}
