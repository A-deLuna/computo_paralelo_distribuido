using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyApi.Controllers
{
    [ApiController]
    public class MyController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        // /register
        public ActionResult<String> Register()
        {
          Console.WriteLine("Register");
          Console.WriteLine(HttpContext.Connection.RemoteIpAddress);
          // TODO guardar los ips de los workers.
          return "ok";
        }

        [HttpGet]
        [Route("start")]
        public ActionResult<String> Start()
        {
          Console.WriteLine("Start");
          // TODO mandar mensajes a los workers.
          return "ok";
        }
    }
}
