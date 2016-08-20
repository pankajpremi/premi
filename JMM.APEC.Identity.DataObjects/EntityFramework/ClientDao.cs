using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class ClientDao : IClientDao
    {
        static ClientDao()
        {
            Mapper.CreateMap<tblIdentity_Clients, Client>();
            Mapper.CreateMap<Client, tblIdentity_Clients>();
        }

        public Client GetClientById(string clientId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var client = context.tblIdentity_Clients.FirstOrDefault(c => c.ID == clientId) as tblIdentity_Clients;
                return Mapper.Map<tblIdentity_Clients, Client>(client);
            }
        }
    }
}
