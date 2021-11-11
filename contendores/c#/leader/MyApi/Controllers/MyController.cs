using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyApi.Controllers
{
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;
        private string _ip;

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
          this._ip = HttpContext.Connection.RemoteIpAddress.ToString();
          Console.WriteLine(this._ip);
          // TODO guardar los ips de los workers.
          return "ok";
        }

        [HttpGet]
        [Route("start")]
        public async Task<String> Start()
        {
          Console.WriteLine("Start");
          using (var client = new HttpClient())
          {
              Console.WriteLine(this._ip);
              var uristring  = "http://[" + this._ip + "]:8080/work";
              Console.WriteLine(uristring);
              var uri = new Uri(uristring);
              var response = await client.GetAsync(uri);
              string textResult = await response.Content.ReadAsStringAsync();
              Console.WriteLine(textResult);
          }
          // TODO mandar mensajes a los workers.
          return "ok";
        }
    }
}
