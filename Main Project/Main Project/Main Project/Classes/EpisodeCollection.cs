using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Main_Project
{
    public class EpisodeCollection: IEnumerator
    {
        private ObservableCollection<EpisodeInfo> _episodes = new ObservableCollection<EpisodeInfo>();

        public ObservableCollection<EpisodeInfo> Episodes
        {
            get { return _episodes; }
            set { _episodes = value; }
        }

        int position = -1;

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext()
        {
            position++;
            return (position < Episodes.Count);
        }

        public void Reset()
        { 
            position = -1;
        }

        //IEnumerable
        public object Current
        {
            get { return Episodes[position]; }
        }
    }
}
