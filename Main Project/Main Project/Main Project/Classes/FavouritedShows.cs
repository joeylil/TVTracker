using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Main_Project
{
    public sealed class FavouritedShows : INotifyPropertyChanged
    {
        private static FavouritedShows _instance = null;

        public static FavouritedShows Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FavouritedShows();
                }
                return _instance;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
       

        private ObservableCollection<Favourites> _FavouriteShowsList;

        public ObservableCollection<Favourites> FavouriteShowsList
        {
            get
            {
                return _FavouriteShowsList;
            }
            set
            {
                _FavouriteShowsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FavouriteShowsList"));
            }
        }

        public int DatabaseIDCount;
    }
}
