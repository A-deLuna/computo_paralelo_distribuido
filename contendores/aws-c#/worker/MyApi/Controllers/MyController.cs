using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace MyApi.Controllers
{
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;
        private static AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast1);


        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("work")]
        public async Task<int[]> Work(WorkRequest w)
        {
          Console.WriteLine("work " +  w.Start  + " " +  w.End);
          var bucket = "computo-paralelo-distribuido-cetys-2021";
          var key = Environment.GetEnvironmentVariable("ARCHIVO_PALABRAS");
          string content = await GetContents(bucket, key, w.Start, w.End);
          // TODO Process file content
          //
          return arr;
        }

        public struct WorkRequest {
          public long Start {get; set;}
          public long End {get; set;}
        }

        async Task<string> GetContents(string bucket, string key, long start, long end)
        {
            var req = new GetObjectRequest();
            req.BucketName = bucket;
            req.Key = key;
            req.ByteRange = new Amazon.S3.Model.ByteRange(start, end);
            var resp = await MyController.client.GetObjectAsync(req, new CancellationToken(false));
            using var reader = new StreamReader(resp.ResponseStream);
            return reader.ReadToEnd();
        }

    }
}
