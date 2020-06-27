using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Main_Project
{
    public class SeasonData
    {
        ObservableCollection<EpisodeInfo> _season = new ObservableCollection<EpisodeInfo>();

        public ObservableCollection<EpisodeInfo> Season
        {
            get { return _season; }
        }
    }
}
