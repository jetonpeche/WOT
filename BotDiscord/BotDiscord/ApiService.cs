using System.Text;
using System.Text.Json;

namespace BotDiscord
{
    public static class ApiService
    {
        private static string MEDIA_TYPE { get; } = "application/json";

        private static HttpClient http = new()
        {
            BaseAddress = new Uri("http://localhost:5086")
        };

        /// <summary>
        ///  GET
        /// </summary>
        /// <typeparam name="T">Type retour</typeparam>
        /// <param name="apiType"></param>
        /// <param name="route">route sans le / du debut (exemple => lister)</param>
        public async static Task<T> GetAsync<T>(EApiType apiType, string route)
        {
            var reponse = await http.GetAsync($"{apiType}/{route}");

            if(reponse.IsSuccessStatusCode)
            {
                string retour = await reponse.Content.ReadAsStringAsync();
                Console.WriteLine(retour);

                return JsonSerializer.Deserialize<T>(retour)!;
            }
            else
                return default(T);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <typeparam name="T">Type retour</typeparam>
        /// <param name="apiType"></param>
        /// <param name="route">route sans le / du debut (exemple => lister)</param>
        /// <param name="jsonString">Les infos données</param>
        public async static Task<T> PostAsync<T>(EApiType apiType, string route, string jsonString)
        {
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

            var reponse = await http.PostAsync($"{apiType}/{route}", httpContent);

            if (reponse.IsSuccessStatusCode)
            {
                string retour = await reponse.Content.ReadAsStringAsync();
                Console.WriteLine(retour);

                return JsonSerializer.Deserialize<T>(retour)!;
            }
            else
                return default(T);
        }
    }
}
