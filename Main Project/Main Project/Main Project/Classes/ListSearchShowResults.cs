using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Main_Project
{
    public class ListSearchShowResults: INotifyPropertyChanged
    {

        private ObservableCollection<SearchShowResult> _searchResults;

        public ObservableCollection<SearchShowResult> searchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("searchResults"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;       
    }
}
