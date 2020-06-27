using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Main_Project
{
    public class Show : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("officialSite")]
        public string OfficialSite { get; set; }

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
        public long? Runtime { get; set; }

        [JsonProperty("premiered")]
        public DateTimeOffset? Premiered { get; set; }

        [JsonProperty("schedule")]
        public Schedule Schedule { get; set; }

        [JsonProperty("rating")]
        public Rating Rating { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("webChannel")]
        public Network WebChannel { get; set; }

        public string GetAirSite
        {
            get
            {
                if (OfficialSite != null)
                {
                    return "<b>Available at: </b>" + OfficialSite;
                }
                else
                {
                    return "No current site";
                }
            }
        }

        public string GetCountryCode
        {
            get
            {
                if(Network != null)
                {
                    return "<b>Region:</b> "  + Network.Country.Code;
                }
                else
                {
                    return "<b>Region:</b> " + WebChannel.Country.Code;
                }
            }
        }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        public EpisodeCollection EpisodeCollection { get; set; }

        public bool HasEpisodeCollection
        {
            get
            {
                if (EpisodeCollection is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public DateTimeOffset? getAirdateForSorting
        {
            get
            {
                if (HasEpisodeCollection)
                {
                    return EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime;
                }
                else
                {
                    return DateTimeOffset.MaxValue;
                }
            }
        }

        public string DisplayAirdate
        {
            get
            {
                if (HasEpisodeCollection)
                {
                    if(DateTime.Now.Day == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Day && DateTime.Now.Month == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Month && DateTime.Now.Year == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Year)
                    {
                        return "<b>Next Episode:</b> Today at " + EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.ToString("hh:mmtt");
                    }
                    else if(DateTime.Now.Day+1 == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Day && DateTime.Now.Month == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Month && DateTime.Now.Year == EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.Year)
                    {
                            return "<b>Next Episode:</b> Tommorow at " + EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.ToString("hh:mmtt");
                    } 
                    else 
                    {
                        return "<b>Next Episode:</b> " + EpisodeCollection.Episodes[0].AirStamp.Value.LocalDateTime.ToString("ddd dd/MM/yyyy hh:mmtt");
                    }
                }
                return "No current schedule for your region";
            }
        }

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

        public string DisplayScheduleTime
        {
            get
            {
                if (Schedule.Time != "")
                {
                    if (Network != null)
                    {
                        return string.Format("<b>Time:</b> {0}  {1}", DateTime.ParseExact(Schedule.Time, "HH:mm", CultureInfo.CurrentCulture).ToString("hh:mm tt"), Network.Country.Timezone);
                    }
                    else if (WebChannel != null)
                    {
                        return string.Format("<b>Time:</b> {0}  {1}", DateTime.ParseExact(Schedule.Time, "HH:mm", CultureInfo.CurrentCulture).ToString("hh:mm tt"), WebChannel.Country.Timezone);
                    }
                    else
                    {
                        return string.Format("<b>Time:</b> {0}", DateTime.ParseExact(Schedule.Time, "HH:mm", CultureInfo.CurrentCulture).ToString("hh:mm tt"));
                    }
                }
                else
                {
                    return "No Scheduled Timeslots";
                }
            }
        }
    }
}
