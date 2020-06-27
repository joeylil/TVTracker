using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Main_Project
{
    public partial class ShowInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("genres")]
        public string[] Genres { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        [JsonProperty("premiered")]
        public string _Premiered { get; set; }

        public string Premiered
        {
            get
            {
                if (_Premiered != null)
                {
                    string dateOfPremiered = _Premiered.Substring(8, 2) + "/" + _Premiered.Substring(5, 2) + "/" + _Premiered.Substring(0, 4);
                    return dateOfPremiered;
                }
                else return null;
            }
        }

        [JsonProperty("officialSite")]
        public Uri OfficialSite { get; set; }

        [JsonProperty("schedule")]
        public Schedule Schedule { get; set; }

        [JsonProperty("rating")]
        public Rating Rating { get; set; }

        [JsonProperty("weight")]
        public long? Weight { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("webChannel")]
        public object WebChannel { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        public Uri getImage
        {
            get
            {
                if (Image != null)
                {
                    return Image.Medium;
                }
                else
                {
                    return new Uri("https://previews.123rf.com/images/pavelstasevich/pavelstasevich1811/pavelstasevich181101028/112815904-no-image-available-icon-flat-vector-illustration.jpg");
                }
            }
        }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("updated")]
        public long? Updated { get; set; }

        private bool _IsInFavouriteList;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsInFavouriteList
        {
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
    }
}
