using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AspNetExam.DTOs
{
    public class EarthquakesCollectionDTO
    {
        [JsonProperty(PropertyName = "features")]
        public List<EarthquakeDTO> Earthquakes { get; set; }
    }
}
