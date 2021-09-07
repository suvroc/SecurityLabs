using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecurityLabs.Services;

namespace SecurityLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedosController : ControllerBase
    {
        private readonly IMailValidationServicecs mailValidationServicecs;

        public RedosController(IMailValidationServicecs mailValidationServicecs)
        {
            this.mailValidationServicecs = mailValidationServicecs;
        }

        [HttpGet]
        [Route("match")]
        public ReDosResult Match(string inputText)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            mailValidationServicecs.Validate(inputText);

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