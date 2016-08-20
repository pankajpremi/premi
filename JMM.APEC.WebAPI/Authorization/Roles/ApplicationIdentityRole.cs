using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class ApplicationIdentityRole : IRole<int>
    {
        public ApplicationIdentityRole()
        {
            Id = 0;
        }

        public ApplicationIdentityRole(string name) : this()
        {
            Name = name;
        }

        public ApplicationIdentityRole(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public ApplicationIdentityRole(string name, int id, string serviceid)
        {
            Name = name;
            Id = id;
            ServiceId = serviceid;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ServiceId { get; set; }

    }
}