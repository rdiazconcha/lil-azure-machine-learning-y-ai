using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace vision
{
    class Program
    {
        static void Main(string[] args)
        {
            string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
            string key = "c50bac85176f4da384d8e2d06bf17d41";

            var credentials = new ApiKeyServiceClientCredentials(key);
            var client = new ComputerVisionClient(credentials);
            client.Endpoint = endpoint;

            //Detección de objetos

            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.jpg");

            if (!files.Any()){
                System.Console.WriteLine("No hay imágenes");
                return;
            }

            var features = new List<VisualFeatureTypes>();
            features.Add(VisualFeatureTypes.Objects);
            features.Add(VisualFeatureTypes.Description);
            features.Add(VisualFeatureTypes.Tags);

            foreach (var file in files){
                using (Stream stream = File.OpenRead(file)){
                    var analysis = client.AnalyzeImageInStreamAsync(stream, features).Result;

                    System.Console.WriteLine($"Archivo: {file}");
                    System.Console.WriteLine(analysis.Description?.Captions?.FirstOrDefault().Text);
                    System.Console.WriteLine(analysis.Objects?.FirstOrDefault()?.ObjectProperty);
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                }
            }

            //Detección de texto

            files = Directory.GetFiles(Environment.CurrentDirectory, "texto*.jpg");

            foreach (var file in files)
            {

                using (Stream stream = File.OpenRead(file))
                {

                    var analysis = client.BatchReadFileInStreamAsync(stream, TextRecognitionMode.Printed).Result;

                    string operationId = analysis.OperationLocation.Substring(analysis.OperationLocation.Length - 36);

                    var result = client.GetReadOperationResultAsync(operationId).Result;

                    while (result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted)
                    {
                        System.Console.WriteLine("Esperando...");
                        Task.Delay(1000).Wait();

                        result = client.GetReadOperationResultAsync(operationId).Result;

                    }


                    if (result.RecognitionResults == null)
                    {
                        continue;
                    }

                    foreach ( var recognition in result.RecognitionResults)
                    {

                        foreach (var line in recognition?.Lines)
                        {
                            System.Console.WriteLine(line.Text);

                        }

                    }

                }

            }






        }
    }
}
