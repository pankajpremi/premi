using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IUser interface 
    /// </summary>
    public class ApplicationIdentityUser : IUser<int>
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public ApplicationIdentityUser()
        {
            Id = 0;
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public ApplicationIdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool Approved { get; set; }
        public string StatusCode { get; set; }
        public DateTime StatusUpdateDateTime { get; set; }
        public DateTime SignUpDate { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual bool PortalAllowed { get; set; }
        public virtual int CurrentPortalId { get; set; }
        public virtual List<ApplicationPortal> Portals { get; set; }

        public virtual List<ApplicationGateway> Gateways { get; set; }

        public virtual IdentityUserProfile Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationIdentityUser, int> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public bool VerifyPortalAccess(string portalurl, ref int portalid)
        {
            this.PortalAllowed = false;

            if (this.Portals != null)
            {
                foreach (var p in this.Portals)
                {
                    foreach (var s in p.DomainUrls)
                    {
                        if (s == portalurl)
                        {
                            this.PortalAllowed = true;
                            p.CurrentPortal = true;
                            p.CurrentDomain = s;

                            portalid = p.Id;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}