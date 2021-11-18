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
        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("work")]
        public async Task<int[]> Work(ReqObj req)
        {
          Console.WriteLine("work");
          return new int[128];
        }

        public struct ReqObj {
          public int Something { get; set; }
        }
    }
}
