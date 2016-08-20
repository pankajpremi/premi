using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using JMM.APEC.Common.Interfaces;
using JMM.APEC.Core;

namespace JMM.APEC.Common
{
    public class ApecDatabase : IDisposable
    {

        private string _dataProvider;
        private string _databaseName;

        public ApecDatabase()
            : this("ApecDataProvider")
        {

        }

        public ApecDatabase(string providerName)
        {
            string provider = ConfigurationManager.AppSettings.Get(providerName);
            _dataProvider = provider;
            _databaseName = "Apec2DatabaseJMM";
        }

        public ApecDatabase(string providerName, ApplicationSystemUser currentUser)
        {
            string provider = ConfigurationManager.AppSettings.Get(providerName);
            _dataProvider = provider;

            if (currentUser == null)
            {
                _databaseName = "Apec2DatabaseJMM";
            }
            else
            {
                _databaseName = currentUser.Portal.ConnectionName;
            }
        }

        public IDaoFactory GetFactory()
        {
            // return the requested DaoFactory

            switch (_dataProvider.ToLower())
            {
                //case "ado.net": return new AdoNet.DaoFactory();
                case "enterpriselibrary": return new Common.EnterpriseLibrary.DaoFactory(_databaseName);

                default: return new Common.EnterpriseLibrary.DaoFactory(_databaseName);
            }
        }

        public void Dispose()
        {
            //
        }

    }
}
