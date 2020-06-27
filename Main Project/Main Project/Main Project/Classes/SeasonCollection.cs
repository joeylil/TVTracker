using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Main_Project
{
    public class SeasonCollection
    {

        ObservableCollection<SeasonData> _seasons = new ObservableCollection<SeasonData>();

        public ObservableCollection<SeasonData> Seasons
        {
            get { return _seasons; }
        }

        public List<String> seasonDisplay
        {
            get
            {
                List<string> seasons = new List<string>();
                for(int i = 1; i < this.Seasons.Count + 1; i++)
                {
                    seasons.Add(string.Format("Season {0}", i));
                }
                return seasons;
            }
        }

        private int _chosenSeason = 1;

        public int ChosenSeason
        {
            get
            {
                return _chosenSeason;
            }
            set
            {
                _chosenSeason = value;
            }
        }


        public ObservableCollection<EpisodeInfo> DisplayChosenSeason
        {
            get
            {
                return Seasons[ChosenSeason - 1].Season;
            }

        }
    }
}
