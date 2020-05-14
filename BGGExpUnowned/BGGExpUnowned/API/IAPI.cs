using System;
using System.Collections.Generic;
using System.Text;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    public interface IAPI
    {
        List<BoardGame> GetCollectionWithoutExpansions(string username);
        List<BoardGame> GetExpansionsOfGames(IEnumerable<long> IDs);
    }
}
