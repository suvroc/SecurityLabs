using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecurityLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SsrsController : ControllerBase
    {
        [HttpGet]
        [Route("load")]
        public SsrsResult Load([FromQuery] string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string ret = client.DownloadString(url);

                    return new SsrsResult()
                    {
                        Response = ret.Length >= 1000 ? ret.Substring(0, 4000) : ret
                    };

                }
                catch (Exception ex)
                {
                    return new SsrsResult()
                    {
                        Response = "Error"
                    };
                }
            }
        }
    }

    public class SsrsResult
    {
        public string Response { get; set; }
    }
}