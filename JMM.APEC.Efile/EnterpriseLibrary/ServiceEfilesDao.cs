//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
//using JMM.APEC.DataObjects.Interfaces;
using JMM.APEC.Core;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.Efile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JMM.APEC.Efile.EnterpriseLibrary
{
    public class ServiceEfilesDao : IServiceEfilesDao
    {
        private string _databaseName;
        private Db db;

        public ServiceEfilesDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        public List<Service_EfileNode> GetEFileItems(ApplicationSystemUser user, string Gateways, string SortOption, string Keyword)
        {
            if (SortOption.ToUpper() == "SERVICES")
            {
                XDocument xdoc = XDocument.Load(@"C:\\TFSProjects2015\\EFile_ByService.xml");

                var nodes = xdoc.Root.Elements().Select(e => Service_EfileNode.Parse(e)).ToList();

                return nodes;
            }
            else if(SortOption.ToUpper() == "FACILITIES")
            {
                XDocument xdoc = XDocument.Load(@"C:\\TFSProjects2015\\EFile_ByFacility.xml");

                var nodes = xdoc.Root.Elements().Select(e => Service_EfileNode.Parse(e)).ToList();

                return nodes;
            }

            return null;
        }

    }
}
