using botDiscord.Enums;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace botDiscord;

public static class ApiService
{
    private const string MEDIA_TYPE = "application/json";
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
    public async static Task<T?> GetAsync<T>(EApiType apiType, string route, JsonTypeInfo<T> _jsonContext)
    {
        var reponse = await http.GetAsync($"{apiType}/{route}");

        if (reponse.IsSuccessStatusCode)
            return (await JsonSerializer.DeserializeAsync(await reponse.Content.ReadAsStreamAsync(), _jsonContext))!;

        else
            return default;
    }

    /// <summary>
    /// POST
    /// </summary>
    /// <typeparam name="T">Type retour</typeparam>
    /// <param name="apiType"></param>
    /// <param name="route">route sans le / du debut (exemple => lister)</param>
    /// <param name="jsonString">Les infos données</param>
    /// <param name="_jsonContext">Le context de serialization des infos données (json context)</param>
    public async static Task<T?> PostAsync<T, U>(EApiType _apiType, string _route, object _json, JsonTypeInfo<U> _jsonContext)
    {
        HttpContent httpContent = new StringContent(JsonSerializer.Serialize(_json, _jsonContext), Encoding.UTF8, MEDIA_TYPE);

        var reponse = await http.PostAsync($"{_apiType}/{_route}", httpContent);

        if (reponse.IsSuccessStatusCode)
            return (await JsonSerializer.DeserializeAsync<T>(await reponse.Content.ReadAsStreamAsync()))!;

        else
            return default;
    }

    /// <summary>
    /// POST
    /// </summary>
    /// <param name="apiType"></param>
    /// <param name="route">route sans le / du debut (exemple => lister)</param>
    /// <param name="route">route sans le / du debut (exemple => lister)</param>
    /// <param name="_json">Les infos données</param>
    /// <param name="_jsonContext">Le context de serialization des infos données (json context)</param>
    public async static Task<HttpResponseMessage?> PostAsync<T>(EApiType _apiType, string _route, object _json, JsonTypeInfo<T> _jsonContext)
    {
        HttpContent httpContent = new StringContent(JsonSerializer.Serialize(_json, _jsonContext), Encoding.UTF8, MEDIA_TYPE);

        var reponse = await http.PostAsync($"{_apiType}/{_route}", httpContent);

        if (reponse.StatusCode is System.Net.HttpStatusCode.InternalServerError)
            return null;
        else
            return reponse;
    }

    /// <summary>
    /// DELETE
    /// </summary>
    /// <param name="apiType"></param>
    /// <param name="route">route sans le / du debut (exemple => lister)</param>
    /// <param name="jsonString">Les infos données</param>
    /// <param name="_jsonContext">Le context de serialization des infos données (json context)</param>
    public async static Task<HttpResponseMessage?> DeleteAsync<T>(EApiType apiType, string route, object _json, JsonTypeInfo<T> _jsonContext)
    {
        var reponse = await http.SendAsync(new(HttpMethod.Delete, $"{apiType}/{route}")
        {
            Content = new StringContent(JsonSerializer.Serialize(_json, _jsonContext), Encoding.UTF8, MEDIA_TYPE)
        });

        if (reponse.StatusCode is System.Net.HttpStatusCode.InternalServerError)
            return null;
        else
            return reponse;
    }

    /// <summary>
    /// DELETE
    /// </summary>
    /// <param name="apiType"></param>
    /// <param name="route">route sans le / du debut (exemple => lister)</param>
    /// <param name="jsonString">Les infos données</param>
    public async static Task<HttpResponseMessage?> DeleteAsync(EApiType apiType, string route)
    {
        var reponse = await http.DeleteAsync($"{apiType}/{route}");

        if (reponse.StatusCode is System.Net.HttpStatusCode.InternalServerError)
            return null;
        else
            return reponse;
    }
}
