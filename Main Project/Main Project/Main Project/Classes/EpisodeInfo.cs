using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
        public partial class EpisodeInfo
        {            
            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("season")]
            public long? Season { get; set; }

            public string DisplayEpisode
            {
            get
                {
                return string.Format("<b>Season:</b> {0} <b>Episode:</b> {1} <b>Runtime:</b> {2}", Season, Episode, Runtime);
                }
            }

            [JsonProperty("number")]
            public long? Episode { get; set; }           

            [JsonProperty("airstamp")]
            public DateTimeOffset? AirStamp { get; set; }


        [JsonProperty("runtime")]
            private string _Runtime;
            public string Runtime
            {
                get
                { 
                    if (_Runtime != null)
                    {
                        return _Runtime + "m";
                    }
                    else
                    {
                        return "N/A";
                    }
                }
                set { _Runtime = value; } 
            }

            [JsonProperty("image")]
            public Image Image { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }
        }
}
