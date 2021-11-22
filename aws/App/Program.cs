using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using System.Threading;

namespace App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

            var bucket = "computo-paralelo-distribuido-cetys-2021";
            var key = "palabras_corto";

            var length = await GetLength(s3Client, bucket, key);
            Console.WriteLine("Total Length: " + length);

            int sections = 10000;
            var rand = new Random();
            var i = rand.Next(sections);
            (long start, long end) r = GetRange(length, sections, i);

            Console.WriteLine($"Fetching section {i} of {sections} ", i, sections);
            Console.WriteLine($"Fetching {r.end - r.start + 1} bytes from {r.start} to {r.end} ", r.start, r.end);

            var content = await GetContents(s3Client, bucket, key, r.start, r.end);
            Console.WriteLine("Read Length: " + content.Length);
            Console.WriteLine("Content:");
            Console.WriteLine(content);
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

        static async Task<long> GetLength(AmazonS3Client s3Client, string bucket, string key)
        {
            var resp = await s3Client.GetObjectMetadataAsync(bucket, key, new CancellationToken(false));
            return resp.ContentLength;
        }

        static async Task<string> GetContents(AmazonS3Client s3Client, string bucket, string key, long start, long end) 
        {
            var req = new GetObjectRequest();
            req.BucketName = bucket;
            req.Key = key;
            req.ByteRange = new Amazon.S3.Model.ByteRange(start, end);
            var resp = await s3Client.GetObjectAsync(req, new CancellationToken(false));
            using var reader = new StreamReader(resp.ResponseStream);
            return reader.ReadToEnd();
        }
    }
}
