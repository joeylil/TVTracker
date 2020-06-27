using Main_Project.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    class CountrySchedule
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("season")]
        public long? Season { get; set; }

        [JsonProperty("number")]
        public long? Episode { get; set; }

        [JsonProperty("airdate")]
        public DateTimeOffset Airdate { get; set; }

        [JsonProperty("airtime")]
        public string Airtime { get; set; }

        [JsonProperty("airstamp")]
        public DateTimeOffset Airstamp { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
    }
}


