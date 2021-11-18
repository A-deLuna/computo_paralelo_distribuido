using System;
using System.Net.Http.Json;
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

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        public async Task<String> Register()
        {
          Console.WriteLine("Register");
          // TODO guardar los ips de los workers.
          Console.WriteLine(HttpContext.Connection.RemoteIpAddress.ToString());
          return "ok";
        }

        [HttpGet]
        [Route("start")]
        public async Task<Response> Start()
        {
          Console.WriteLine("Start");
          // TODO mandar mensajes a los workers.
          //
          // using (var client = new HttpClient())
          // {
          //   var response = await client.PostAsJsonAsync(uri, message);
          //   var counts = await response.Content.ReadFromJsonAsync<int[]>();
          // }
          return new Response {
            MasComun = new CharFreq {
              Character = 'e',
              Count = 1000
            },
            MenosComun = new CharFreq {
              Character = 'z',
              Count = 7
            }
          };
        }
        public struct CharFreq {
          public char Character { get; set; }
          public int Count { get; set; }
        }
        public struct Response {
          public CharFreq MasComun { get; set; }
          public CharFreq MenosComun { get; set; }
        }
    }
}
