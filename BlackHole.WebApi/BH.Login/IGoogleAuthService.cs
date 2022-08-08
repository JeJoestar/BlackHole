using BlackHole.DAL;
using Google.Apis.Auth;

namespace BlackHole.Login
{
    public interface IGoogleAuthService
    {
        Task<User> Authenticate(GoogleJsonWebSignature.Payload payload);
    }
}