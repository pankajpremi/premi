using JMM.APEC.WebAPI.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.Providers
{
    public class CustomUserValidator<TUser, TKey> : IIdentityValidator<TUser>
        where TUser : ApplicationIdentityUser
    {
        private static readonly Regex EmailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly UserManager<TUser, int> _manager;

        public CustomUserValidator()
        {

        }

        public CustomUserValidator(UserManager<TUser, int> manager)
        {
            _manager = manager;
        }

        public async Task<IdentityResult> ValidateAsync(TUser item)
        {
            var errors = new List<string>();

            if (!EmailRegex.IsMatch(item.UserName))
                errors.Add("Enter a valid email address as username.");

            if (_manager != null)
            {
                var otherAccount = await _manager.FindByNameAsync(item.UserName);
                if (otherAccount != null && otherAccount.Id != item.Id)
                    errors.Add("Select a different email address.  An account already exists with this email address.");
            }

            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;

        }
    }
}