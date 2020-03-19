using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using com.mbpro.BGGExpUnowned.API;
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
			//TODO:
			/*
			 * retrieve collection without expansions
			 * Put into  hashmap (to exclude doubles)
			 * For each board game, retrieve expansions. Put into hashmap
			 * Retrieve collection only expansions
			 * Put into the first hashmap
			 * Check for each in the expansions hashmap if it's in the collection hashmap. If not -> unowned!
			 * 
			 * Implement retries for http requests
			 * 
			 * Optional:
			 * Add filter for everything on your wishlist
			 * Replace ListBox with sortable table?
			 * 
			 */
		}
	}
}
