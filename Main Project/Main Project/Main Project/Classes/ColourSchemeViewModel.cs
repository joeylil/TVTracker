using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Main_Project.Classes
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            SchemeColourList = new ObservableCollection<string>
            {
                "Blue",
                "Orange",
                "Pink",
            };

            RegionList = new ObservableCollection<string>
            {
                "Australia",
                "United Kingdom",
                "America",
                "Russian Federation",
                "China",
                "Ireland",
                "Japan",
                "Korea, Republic of",
            };
        }


        ObservableCollection<string> _SchemeColourList;
        ObservableCollection<string> _RegionList;


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> SchemeColourList
        {
            get { return _SchemeColourList; }
            private set
            {
                _SchemeColourList = value;
            }
        }
        public ObservableCollection<string> RegionList
        {
            get { return _RegionList; }
            private set
            {
                _RegionList = value;
            }
        }

        private string _SelectedColour;

        public string SelectedColour
        {
            get
            {
                return _SelectedColour;
            }

            set
            {
                _SelectedColour = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedColour"));

            }
        }

        private string _SelectedRegion;

        public string SelectedRegion
        {
            get
            {
                return _SelectedRegion;
            }

            set
            {
                _SelectedRegion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedRegion"));
            }
        }

        private Color _SubmitButtonColour;

        public Color SubmitButtonColour
        {
            get
            {
                return _SubmitButtonColour;
            }

            set
            {
                _SubmitButtonColour = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SubmitButtonColour"));
            }
        }

    }
}
