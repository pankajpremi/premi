using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<ApplicationIdentityRole, int>
    {
        public ApplicationRoleManager(ApplicationRoleStore<ApplicationIdentityRole, int> roleStore) : base(roleStore)
        {

        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new ApplicationRoleStore<ApplicationIdentityRole, int>(context.Get<ApplicationDbContext>() as IdentityDatabase));

            return appRoleManager;
        }

    }
}