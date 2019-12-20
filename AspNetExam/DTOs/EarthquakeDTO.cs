using AspNetExam.Models;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AspNetExam.DTOs
{
    public class EarthquakeDTO
    {
        [JsonProperty(PropertyName = "properties")]
        public EarthquakeInfoDTO EarthquakeInfo { get; set; }
    }
}