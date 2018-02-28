namespace Excellent
{
    using System.Net.Http;
    using System.Speech.Synthesis;
    using System.Threading.Tasks;
    using Interfaces;
    using Newtonsoft.Json;
    using Properties;

    public class CatFactsService : ICatFactsService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public Task Get()
        {
            return Task.Run(
                async () =>
                {
                    using (var synth = new SpeechSynthesizer())
                    {
                        var response = await httpClient.GetStringAsync(Resources.CatFactsUrl);

                        var catFact = JsonConvert.DeserializeAnonymousType(
                            response,
                            new
                            {
                                fact = string.Empty
                            });


                        synth.Speak(catFact.fact);                        
                    }
                });
        }
    }
}