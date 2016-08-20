using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Controllers
{
    [RoutePrefix("api/refreshtoken")]
    public class TokenController : BaseApiController
    {
        private IdentityDatabase database = null;

        public TokenController()
        {
            database = new IdentityDatabase();
        }

        [Authorize(Roles = "SUPER ADMIN,ADMIN")]
        [Route("")]
        public IHttpActionResult Get()
        {
            RefreshTokenTable tbl = new RefreshTokenTable(database);
            return Ok(tbl.GetAllRefreshTokens());
        }

        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            RefreshTokenTable tbl = new RefreshTokenTable(database);

            var result = await tbl.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest(Resources.LangResource.TokenId);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
