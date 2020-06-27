using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Main_Project
{
    [Table("FavouriteShows")]
    public class Favourites
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        private string _ShowID;

        [Unique]
        public string ShowID
        {
            get => _ShowID;
            set
            {
                _ShowID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("title"));
            }
        }
    }   
}
