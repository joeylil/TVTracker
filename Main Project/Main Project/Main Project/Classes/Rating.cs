using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    public partial class Rating
{
    [JsonProperty("average")]
    public string Average { get; set; }
}
}
