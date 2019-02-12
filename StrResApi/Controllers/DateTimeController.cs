using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StrResApi.Controllers
{
    [Route("api/[controller]")]
    public class DateTimeController : Controller
    {
        // GET api/datetime
        [HttpGet]
        public string Get()
        {
            var now = DateTime.Now;

            return JsonConvert.SerializeObject(new { date = now.ToLongDateString(), time = now.ToLongTimeString() });
        }
    }
}
