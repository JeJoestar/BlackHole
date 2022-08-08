namespace BlackHole.Login
{
    public interface ITwitterAuthService
    {
        Task<RequestTokenResponse> GetRequestToken();

        Task<UserModelDto> GetAccessToken(string oauth_request, string oauth_verifier);
    }
}