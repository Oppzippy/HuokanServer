using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.ApiKeyRepository;

public interface IApiKeyRepository
{
	Task<string> CreateApiKey(ApiKey apiKey);
	Task<BackedApiKey> FindApiKey(string apiKey);
}