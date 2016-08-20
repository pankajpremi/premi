using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace JMM.APEC.WebAPI
{
    public class Utility
    {

       
        public static ClaimsIdentity TryGetUserClaims()
        {
            if (HttpContext.Current == null)
                return null;

            var context = HttpContext.Current.GetOwinContext();
            if (context == null)
                return null;

            if (context.Authentication == null || context.Authentication.User == null)
                return null;

            ClaimsPrincipal principal =  context.Authentication.User;

            ClaimsIdentity identity = principal.Claims as ClaimsIdentity;

            return identity;

        }




        public static Claim TryGetClaim(ClaimsPrincipal owinUser, string key)
        {
            if (owinUser == null)
                return null;

            if (owinUser.Claims == null)
                return null;

            return owinUser.Claims.FirstOrDefault(o => o.Type.Equals(key));
        }
    }
}
