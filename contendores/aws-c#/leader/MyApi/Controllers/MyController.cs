using System;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;


namespace MyApi.Controllers
{
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;
        public static List<string> _ip = new List<string>();
        private static AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast1);

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        // /register
        public ActionResult<String> Register()
        {
          var ip = HttpContext.Connection.RemoteIpAddress.ToString();
          Console.WriteLine("Register " + ip);
          MyController._ip.Add(ip);
          return "ok";
        }

        [HttpGet]
        [Route("start")]
        public async Task<Response> Start()
        {
          Stopwatch sw = new Stopwatch();
          sw.Start();

          Console.WriteLine("Start");
          int[] responses = new int[128];

          var bucket = "computo-paralelo-distribuido-cetys-2021";
          var key = Environment.GetEnvironmentVariable("ARCHIVO_PALABRAS");

          var length = await GetLength(bucket, key);
          int n = MyController._ip.Count();
          using (var client = new HttpClient())
          {
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
            for (int i = 0; i < n; i++) {
              // TODO implement sending requests
              tasks.Add(client.PostAsJsonAsync(uri, wr));
            }
            var results = await Task.WhenAll(tasks);
            foreach (var response in results) {
              var resp = await response.Content.ReadFromJsonAsync<int[]>();
              //TODO Aggregate responses
              
              Console.WriteLine("client response: ");
              Array.ForEach(resp, Console.Write);
              Console.WriteLine("end");
            }
          }
          sw.Stop();
          // TODO Calculate result

          return new Response {
            MasComun = new CharFreq {
              Character = max,
              Count = nmax
            },
            MenosComun = new CharFreq {
              Character = min,
              Count = nmin
            },
            Runtime = sw.Elapsed.TotalMilliseconds + "ms"
          };
        }
        public struct CharFreq {
          public char Character { get; set; }
          public int Count { get; set; }
        }
        public struct Response {
          public CharFreq MasComun { get; set; }
          public CharFreq MenosComun { get; set; }
          public String Runtime {get; set;}
        }

        public struct WorkRequest {
          public long Start { get; set; }
          public long End { get; set; }
        }

        async Task<long> GetLength(string bucket, string key)
        {
            var resp = await MyController.client.GetObjectMetadataAsync(bucket, key, new CancellationToken(false));
            return resp.ContentLength;
        }

        static ValueTuple<long, long> GetRange(long length, int sections, int i) {
          var sectionLength = length / sections;
          long start, end = 0;
          if (i == sections - 1) {
            start = sectionLength * i;
            end = length-1;
          } else {
            start = sectionLength * i;
            end = sectionLength * (i+1) - 1;
          }
          return (start, end);
        }
    }
}
