using Microsoft.AspNetCore.Identity;

namespace NZWalksApi.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
