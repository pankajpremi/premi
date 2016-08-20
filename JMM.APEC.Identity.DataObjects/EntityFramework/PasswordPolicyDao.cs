using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class PasswordPolicyDao : IPasswordPolicyDao
    {
        static PasswordPolicyDao()
        {
            Mapper.CreateMap<tblIdentity_PasswordPolicies, PasswordPolicy>();
            Mapper.CreateMap<PasswordPolicy, tblIdentity_PasswordPolicies>();
        }

        public PasswordPolicy FindByPortalUrl(string portalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var Policies = (from pp in context.tblIdentity_PasswordPolicies
                                join po in context.tblIdentity_Portals
                                on pp.PortalId equals po.ID
                                where po.DomainURLs.Contains(portalId)
                                select pp).ToList();

                if (Policies != null)
                {
                    var SelectedPolicies = Policies.AsQueryable().Select(u =>
                        new PasswordPolicy
                        {
                            Id = u.Id,
                            PortalId = u.PortalId,
                            RequireDigit = u.RequireDigit,
                            RequiredLength = u.RequiredLength,
                            RequireLowercase = u.RequireLowercase,
                            RequireNonLetterOrDigit = u.RequireNonLetterOrDigit,
                            RequireUppercase = u.RequireUppercase
                        })
                        .ToList();

                    return SelectedPolicies.FirstOrDefault();
                }

                return null;
            }
        }
    }
}
