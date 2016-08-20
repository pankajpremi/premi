using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public class IdentityDatabase : IDisposable
    {

        private string _dataProvider;

        public IdentityDatabase()
            : this("IdentityDataProvider")
        {

        }

        public IdentityDatabase(string providerName)
        {
            string provider = ConfigurationManager.AppSettings.Get(providerName);
            _dataProvider = provider;
        }

        public IDaoFactory GetFactory()
        {
            // return the requested DaoFactory

            switch (_dataProvider.ToLower())
            {
                //case "ado.net": return new AdoNet.DaoFactory();
                case "entityframework": return new EntityFramework.DaoFactory();

                default: return new EntityFramework.DaoFactory();
            }
        }

        public void Dispose()
        {
            //
        }


    }
}
