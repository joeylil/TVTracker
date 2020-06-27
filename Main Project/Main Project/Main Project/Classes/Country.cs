using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    public partial class Country
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }
}
}
