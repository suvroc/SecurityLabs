using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecurityLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedosController : ControllerBase
    {
        [HttpGet]
        [Route("match")]
        public ReDosResult Match(string inputText)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var regex = new Regex(
               @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$",
               RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            // Takes more than 30s om my computer
            regex.IsMatch(inputText);

            stopwatch.Stop();
            return new ReDosResult()
            {
                EplasedTime = stopwatch.Elapsed.Milliseconds,
                EplasedTimeString = stopwatch.Elapsed.ToString(),
                Message = "OK"
            };
        }

        [HttpPost]
        [Route("passwordMatch")]
        public ReDosResult PasswordMatch([FromForm]ReDosModel model)
        {
            // ReDoS via Regex Injection

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Regex testPassword = new Regex(model.User);
            Match match = testPassword.Match(model.Password);

            stopwatch.Stop();

            if (match.Success)
            {
                return new ReDosResult()
                {
                    EplasedTime = stopwatch.Elapsed.Milliseconds,
                    EplasedTimeString = stopwatch.Elapsed.ToString(),
                    Message = "Do not include name in password."
                };
            }
            else
            {
                return new ReDosResult()
                {
                    EplasedTime = stopwatch.Elapsed.Milliseconds,
                    EplasedTimeString = stopwatch.Elapsed.ToString(),
                    Message = "Good password."
                };
            }
        }
    }

    public class ReDosModel
    {
        public string User { get; set; }
        public string Password { get; set; }

    }

    public class ReDosResult
    {
        public long EplasedTime { get; set; }
        public string EplasedTimeString { get; set; }
        public string Message { get; set; }
    }
}