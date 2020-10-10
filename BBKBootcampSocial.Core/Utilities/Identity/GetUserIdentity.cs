using System;
using System.Security.Claims;

namespace BBKBootcampSocial.Core.Utilities.Identity
{
    public static class GetUserIdentity
    {
        public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                return Convert.ToInt64(result.Value);
            }
            return default(long);
        }
    }
}
