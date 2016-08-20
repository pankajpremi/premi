using JMM.APEC.Identity.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class ApplicationDbContext : IdentityDatabase
    {
        public ApplicationDbContext(string providerName)
            : base(providerName)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext("IdentityDataProvider");
        }
    }
}