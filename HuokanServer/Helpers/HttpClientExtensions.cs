using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HuokanServer.Helpers;

public static class HttpClientExtensions
{
	public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(
		this HttpClient client,
		string requestUri,
		T content,
		JsonSerializerOptions jsonSerializerOptions = null
	)
	{
		string json = JsonSerializer.Serialize(content, jsonSerializerOptions);
		var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
		return await client.PatchAsync(requestUri, httpContent);
	}
}
