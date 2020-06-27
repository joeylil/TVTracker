using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    public partial class Network
{
    [JsonProperty("id")]
    public long? Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("country")]
    public Country Country { get; set; }
}
}
