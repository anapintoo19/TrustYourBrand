using System.IdentityModel.Tokens.Jwt;

namespace TrustYourBrand.Services
{
    public static class JwtHelper
    {
        public static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return true;

            var jwt = handler.ReadJwtToken(token);
            var exp = jwt.ValidTo;

            return exp < DateTime.UtcNow;
        }
    }
}