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
		
		private readonly IAPI _api;
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
			await Task.Run(() => SearchAsync()).ConfigureAwait(false);
			EnableSearchButton = true;
		}
		
		private async Task SearchAsync()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Unowned.Clear();
			});
			Username = Username.Trim();
			List<BoardGame> collection = await _api.GetCollectionWithoutExpansionsAsync(Username);
			Dictionary<long, BoardGame> collectionFiltered = ConvertToDictionary(collection);
			List<BoardGame> allExpansions = await _api.GetExpansionsOfGamesAsync(collectionFiltered.Keys);
			Dictionary<long, BoardGame> expansionsOwned = ConvertToDictionary(await _api.GetCollectionOnlyExpansionsAsync(Username));
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
			 * 
			 * Optional:
			 * Add filter for everything on your wishlist (or other lists)
			 * Replace ListBox with sortable table?
			 * Add link in the result so you can visit the game on BGG
			 * Create progress bar
			 * Tests
			 * Add logging
			 * Check if username exists. (query collection api and check for message:
			 * <errors>
<error>
<message>Invalid username specified</message>
</error>
</errors>
			 * 
			 */
		}

		private Dictionary<long, BoardGame> ConvertToDictionary(List<BoardGame> collection)
		{
			Dictionary<long, BoardGame> collectionFiltered = new Dictionary<long, BoardGame>();
			foreach (BoardGame game in collection)
			{
				if (collectionFiltered.ContainsKey(game.ID)) continue;
				collectionFiltered.Add(game.ID, game);

			}
			return collectionFiltered;
		}
	}
}
