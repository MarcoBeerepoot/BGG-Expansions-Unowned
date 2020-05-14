using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using com.mbpro.BGGExpUnowned.API;
using com.mbpro.BGGExpUnowned.model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace com.mbpro.BGGExpUnowned.ViewModels
{
    class MainViewModel : ViewModelBase
    {
		public ICommand SearchCollectionCommand { get;}
		
		private IAPI _api;
		public string username { get; set; }

        public MainViewModel(IAPI api)
        {
           _api = api;
			SearchCollectionCommand = new RelayCommand(SearchButtonClicked);
        }
		private void SearchButtonClicked()
		{
			username = username.Trim();
			List<BoardGame> collection = _api.GetCollectionWithoutExpansions(username);
			Dictionary<long, BoardGame> collectionFiltered = ConvertToDictionary(collection);
			List<BoardGame> expansions = _api.GetExpansionsOfGames(collectionFiltered.Keys);
			//TODO:
			/*
			 * Disable search button while searching.
			 * Is this done on the UI thread? If not use async or whatever I need.
			 * For each board game, retrieve expansions. Put into hashmap
			 * Retrieve collection only expansions
			 * Put into the first hashmap
			 * Check for each in the expansions hashmap if it's in the collection hashmap. If not -> unowned!
			 * 
			 * Implement retries for http requests
			 * Implement error detection for http requests
			 * 
			 * Optional:
			 * Add filter for everything on your wishlist
			 * Replace ListBox with sortable table?
			 * 
			 */
		}

		private Dictionary<long, BoardGame> ConvertToDictionary(List<BoardGame> collection)
		{
			Dictionary<long, BoardGame> collectionFiltered = new Dictionary<long, BoardGame>();
			foreach (BoardGame game in collection)
			{
				collectionFiltered.Add(game.ID, game);

			}
			return collectionFiltered;
		}
	}
}
