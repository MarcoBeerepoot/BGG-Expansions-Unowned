using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    public interface IAPI
    {
        Task<List<BoardGame>> GetCollectionWithoutExpansionsAsync(string username);
        Task<List<BoardGame>> GetExpansionsOfGamesAsync(IEnumerable<long> IDs);
        Task<List<BoardGame>> GetCollectionOnlyExpansionsAsync(string username);
    }
}
