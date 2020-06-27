using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project.Classes
{
    class Embedded
{
        [JsonProperty("show")]
        public Show Show { get; set; }    
}
}
