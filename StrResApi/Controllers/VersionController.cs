using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StrResApi.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        // GET api/version
        [HttpGet]
        public string Get() => JsonConvert.SerializeObject(new { version = "20190212.5" });
    }
}