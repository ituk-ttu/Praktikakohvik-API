namespace PkAPI.Services.Authorization;

public class AuthorizationService : IAuthorizationService
{
    public bool IsAuthorized(IHeaderDictionary header)
    {
        header.TryGetValue("KEY", out var extractedApiKey);
        var apiKey = Environment.GetEnvironmentVariable("KEY");
        return apiKey == null || apiKey.Equals(extractedApiKey);
    }
}
