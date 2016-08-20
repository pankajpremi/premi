using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class PasswordPolicyTable
    {
        private IdentityDatabase _database;
        private IPasswordPolicyDao passwordPolicyDao;

        public PasswordPolicyTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            passwordPolicyDao = factory.PasswordPolicyDao;
        }

        public PortalPasswordPolicy GetByPortalId(string portalId)
        {
            PortalPasswordPolicy policy = new PortalPasswordPolicy();
            policy.PortalId = 0;
            policy.RequiredLength = 6;
            policy.RequireNonLetterOrDigit = true;
            policy.RequireDigit = false;
            policy.RequireLowercase = true;
            policy.RequireUppercase = true;

            if (portalId != null)
            {
                PasswordPolicy oPolicy = null;
                oPolicy = passwordPolicyDao.FindByPortalUrl(portalId);

                if (oPolicy != null)
                {
                    policy = new PortalPasswordPolicy();
                    policy.PortalId = (int)oPolicy.PortalId;
                    policy.RequireDigit = (bool)oPolicy.RequireDigit;
                    policy.RequiredLength = (int)oPolicy.RequiredLength;
                    policy.RequireNonLetterOrDigit = (bool)oPolicy.RequireNonLetterOrDigit;
                    policy.RequireLowercase = (bool)oPolicy.RequireLowercase;
                    policy.RequireUppercase = (bool)oPolicy.RequireUppercase;
                }
            }

            return policy;
        }
    }
}