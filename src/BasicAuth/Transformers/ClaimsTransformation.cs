using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace BasicAuth.Transformers;

public class ClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var hasClaim = principal.Claims.Any(q => q.Type == "level");

        if (!hasClaim)
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim("level", "B"));
        }

        return Task.FromResult(principal);
    }
}
