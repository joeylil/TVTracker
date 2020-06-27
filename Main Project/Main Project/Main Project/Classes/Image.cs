using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    public partial class Image
    {
        [JsonProperty("medium")]
        public Uri Medium { get; set; }
    }
}
