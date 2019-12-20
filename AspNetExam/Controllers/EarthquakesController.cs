using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetExam.DTOs;
using AspNetExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EarthquakesController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> Get(int amount = 5)
        {
            using (var client = new WebClient())
            {
                string jsonString;
                try
                {
                    jsonString = await client.DownloadStringTaskAsync(new Uri($"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&limit={amount}"));
                }
                catch (WebException)
                {
                    return BadRequest();
                }

                var earthquakesCollection = JsonConvert.DeserializeObject<EarthquakesCollectionDTO>(jsonString);

                var earthquakesInfo = new List<EarthquakeInfo>();

                foreach (var item in earthquakesCollection.Earthquakes)
                {
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

                    earthquakesInfo.Add(new EarthquakeInfo
                    {
                        Magnitude = item.EarthquakeInfo.Magnitude,
                        Place = item.EarthquakeInfo.Place,
                        Time = epoch.AddMilliseconds(double.Parse(item.EarthquakeInfo.Time.ToString()))
                    });
                }
                return Ok(earthquakesInfo);
            }

        }
    }
}