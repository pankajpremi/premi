using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class ClientTable
    {
        private IdentityDatabase _database;
        private IClientDao clientDao;

        public ClientTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            clientDao = factory.ClientDao;
        }

        public ApplicationClient GetClientById(string clientId)
        {
            Client oClient = null;
            oClient = clientDao.GetClientById(clientId);

            ApplicationClient cl = null;

            if (oClient != null)
            {
                Models.ApplicationTypes apptype = Models.ApplicationTypes.NativeConfidential;
                if (oClient.ApplicationType == 1)
                {
                    apptype = Models.ApplicationTypes.NativeConfidential;
                }
                else
                {
                    apptype = Models.ApplicationTypes.JavaScript;
                }

                cl = new ApplicationClient();
                cl.Id = oClient.Id;
                cl.Name = oClient.Name;
                cl.ApplicationType = apptype;
                cl.Active = oClient.Active;
                cl.AllowedOrigin = oClient.AllowedOrigin;
                cl.RefreshTokenLifetime = oClient.RefreshTokenLifetime;
                cl.Secret = oClient.Secret;

            }

            return cl;
        }

    }
}