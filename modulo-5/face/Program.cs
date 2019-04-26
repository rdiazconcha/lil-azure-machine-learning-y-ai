using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace face
{
    class Program
    {
        static void Main(string[] args)
        {
            string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
            string key ="c50bac85176f4da384d8e2d06bf17d41";

            var credentials = new ApiKeyServiceClientCredentials(key);
            var client = new FaceClient(credentials);
            client.Endpoint = endpoint;


            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.jpg");

            var attributes = new List<FaceAttributeType>();
            attributes.Add(FaceAttributeType.Age);
            attributes.Add(FaceAttributeType.Gender);
            attributes.Add(FaceAttributeType.Emotion);

            foreach (var file in files)
            {
                using (Stream stream = File.OpenRead(file))
                {
                    var analysis = client.Face.DetectWithStreamAsync(stream, true, false, attributes).Result;

                    foreach (DetectedFace face in analysis)
                    {
                        var age = face.FaceAttributes?.Age.Value;
                        var gender = face.FaceAttributes?.Gender.Value;
                        var happiness = face.FaceAttributes?.Emotion.Happiness;

                        System.Console.WriteLine(age);
                        System.Console.WriteLine(gender);
                        System.Console.WriteLine(happiness);
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                    }

                }

            }
        }
    }
}
