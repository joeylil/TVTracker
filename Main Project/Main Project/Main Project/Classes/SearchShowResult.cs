using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Xamarin.Forms;

namespace Main_Project
{
    public class SearchShowResult : INotifyPropertyChanged
    {
        [JsonProperty("show")]
        public Show show { get; set; }
        public class Show
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("summary")]
            private string Summary { get; set; }

            public string showSummary
            {
                
                get
                {
                    if(Summary == null)
                    {
                        return "No summary available";
                    }
                    else
                    {
                        return Summary;
                    }
                }               
            }
 

            [JsonProperty("image")]
            public Images Image { get; set; }

            public Uri getImage
            {
                get
                {
                    if (Image != null)
                    {
                        return Image.ImageURL;
                    }
                    else
                    {
                        return new Uri("https://previews.123rf.com/images/pavelstasevich/pavelstasevich1811/pavelstasevich181101028/112815904-no-image-available-icon-flat-vector-illustration.jpg");
                    }
                }
            }

            [JsonProperty("rating")]
            public Rating Rating { get; set; }

            [JsonProperty("genres")]
            public List<string> Genres { get; set; }

            [JsonProperty("network")]
            public Network Network { get; set; }

            [JsonProperty("webChannel")]
            public Network WebChannel { get; set; }
        }

        public class Images
        {
            [JsonProperty("medium")]
            public Uri ImageURL { get; set; }                        
        }



        public class Rating
        {
            [JsonProperty("average")]
            private string _Ave;

            public string Ave { 
                get
                {
                    if (_Ave != null)
                    {
                        return "Rating: " + _Ave;
                    }
                    else
                    {
                        return null;
                    }
                } 
            }

        }

        private bool _IsInFavouriteList;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsInFavouriteList {
            get
            {
                return _IsInFavouriteList;
            }
            set
            {
                _IsInFavouriteList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetFavouriteButtonSource"));
            }
        }

        public string SetFavouriteButtonSource
        {
            get
            {
                if (IsInFavouriteList)
                {
                    return "http://www.clker.com/cliparts/O/Z/4/Q/W/Q/favourite-md.png";
                }
                else
                {
                    return "https://img.icons8.com/windows/50/000000/add-to-favorites.png";
                }
            }
        }
    }
}
