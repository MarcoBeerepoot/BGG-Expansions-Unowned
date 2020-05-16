using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using com.mbpro.BGGExpUnowned.API;
using com.mbpro.BGGExpUnowned.model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace com.mbpro.BGGExpUnowned.ViewModels
{
    class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
		public ICommand SearchCollectionCommand { get;}
		
		private IAPI _api;
		public string Username { get; set; }

		public ObservableCollection<BoardGame> Unowned { get;}
		private bool enableSearchButton = true;
		public bool EnableSearchButton
		{
			get
			{
				return this.enableSearchButton;
			}
			set
			{
				this.enableSearchButton = value;
				RaisePropertyChanged("EnableSearchButton");
			}
		}

		public MainViewModel(IAPI api)
        {
           _api = api;
			SearchCollectionCommand = new RelayCommand(SearchButtonClicked);
			Unowned = new ObservableCollection<BoardGame>();
		}

		private async void SearchButtonClicked()
		{
			EnableSearchButton = false;
			await Task.Run(() => Search()).ConfigureAwait(false);
			EnableSearchButton = true;
		}
		
		private void Search()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Unowned.Clear();
			});
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
					Application.Current.Dispatcher.Invoke(() =>
					{
						Unowned.Add(expansion);
					});
				}
			}
			//TODO:
			/*		
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
