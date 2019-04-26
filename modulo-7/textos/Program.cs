using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace textos
{
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private string key;
        public ApiKeyServiceClientCredentials(string key)
        {
            this.key = key;
        }

        public override Task ProcessHttpRequestAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args==null || !args.Any())
            {
                System.Console.WriteLine("Por favor indicar el texto");
                return;
            }

            var text = args[0];


            string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
            string key = "c50bac85176f4da384d8e2d06bf17d41";

            var credentials = new ApiKeyServiceClientCredentials(key);
            var client = new TextAnalyticsClient(credentials);
            client.Endpoint = endpoint;


            var input = new List<LanguageInput>();
            input.Add(new LanguageInput(id: "1", text: text));
            var batch = new LanguageBatchInput(input);
            var analysis   = client.DetectLanguageAsync(false, batch).Result;

            foreach (var document in analysis.Documents)
            {
                var language = document.DetectedLanguages.FirstOrDefault();
                System.Console.WriteLine(language.Iso6391Name);
            }


        }
    }
}
