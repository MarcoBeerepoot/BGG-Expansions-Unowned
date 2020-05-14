using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public string Username { get; set; }

		public ObservableCollection<BoardGame> Unowned { get; set; }
		private bool _isBusySearching = false;

		public MainViewModel(IAPI api)
        {
           _api = api;
			SearchCollectionCommand = new RelayCommand(SearchButtonClicked, CanSearch);
			Unowned = new ObservableCollection<BoardGame>();
		}

		private bool CanSearch()
		{
			return !_isBusySearching;
		}
		private void SearchButtonClicked()
		{
			_isBusySearching = true;
			Username = Username.Trim();
			List<BoardGame> collection = _api.GetCollectionWithoutExpansions(Username);
			Dictionary<long, BoardGame> collectionFiltered = ConvertToDictionary(collection);
			List<BoardGame> allExpansions = _api.GetExpansionsOfGames(collectionFiltered.Keys);
			Dictionary<long, BoardGame> expansionsOwned = ConvertToDictionary(_api.GetCollectionOnlyExpansions(Username));
			foreach (KeyValuePair<long, BoardGame> game in expansionsOwned)
			{
				collectionFiltered[game.Key] = game.Value;
			}
			foreach (BoardGame expansion in allExpansions)
			{
				if (!collectionFiltered.ContainsKey(expansion.ID))
				{
					Unowned.Add(expansion);
				}
			}
			//TODO:
			/*
			 * Test disable search button while searching.
			 * Is this done on the UI thread? If not use async or whatever I need.
			 * 
			 * Show results
			 * Implement retries for http requests
			 * Implement error detection for http requests
			 * 
			 * Optional:
			 * Add filter for everything on your wishlist (or other lists)
			 * Replace ListBox with sortable table?
			 * Add link in the result so you can visit the game on BGG
			 * Create progress bar
			 * 
			 */
			_isBusySearching = false;
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
