using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main_Project
{
    public partial class Schedule
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("days")]
        public string[] Days { get; set; }

        public string DisplayScheduleDays
        {
            get
            {
                if (Days.Length != 0)
                {
                    string days = "";
                    foreach (string day in Days)
                    {
                        days += " " + day;
                    }
                    if (days == " Monday Tuesday Wednesday Thursday Friday Saturday Sunday")
                    {
                        return "<b>Showing:</b> Everyday";
                    }
                    else if (days == " Monday Tuesday Wednesday Thursday Friday")
                    {
                        return "<b>Showing:</b> Weekdays";
                    }
                    return string.Format("<b>Showing:</b>{0}", days);
                }
                else
                {
                    return "No Scheduled Days";
                }
            }
        }        
    }
}
