namespace PkAPI.Services.Authorization;

public interface IAuthorizationService
{
    bool IsAuthorized(IHeaderDictionary header);
}
