using System;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

namespace customvision
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = "fe3cedd23a9a480fb25bd91184ebb681";
            string endpoint = "https://westeurope.api.cognitive.microsoft.com";

            var client = new CustomVisionPredictionClient();
            client.ApiKey = apiKey;
            client.Endpoint = endpoint;

            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.jpg");

            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                {
                    var result = client.ClassifyImage(Guid.Parse("5da6478e-b413-4512-be92-1c4bb502893f"),
                    "Iteration2", stream);

                    foreach (var p in result.Predictions)
                    {
                        System.Console.WriteLine($"{p.TagName} {p.Probability:P4}");
                    }
                }
            }
        }
    }
}
