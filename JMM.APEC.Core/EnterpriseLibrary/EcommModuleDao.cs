using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
//using JMM.APEC.DataObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class EcommModuleDao : IEcommModuleDao
    {
        private string _databaseName;
        private Db db;

        public EcommModuleDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Portal object based on DataReader
        static Func<IDataReader, Ecomm_ModuleService> MakeModule = reader =>
           new Ecomm_ModuleService
           {
               ModuleId = reader["moduleId"].AsInt(),
               ModuleCode = reader["moduleCode"].AsString(),
               ModuleName = reader["moduleName"].AsString(),
               ModuleDescription = reader["moduleDescription"].AsString(),
               ServiceId = reader["serviceId"].AsInt(),
               ServiceCode = reader["serviceCode"].AsString(),
               ServiceName = reader["serviceName"].AsString(),
               ServiceDescription = reader["serviceDescription"].AsString()
           };

        public List<Ecomm_Module> GetModulesForUser(ApplicationSystemUser user, int? gatewayId)
        {
            string storedProcName = "Ecomm_getModuleService";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inPortalID", DbType = DbType.String, Value = user.Portal.LocalPortalId }

            };

            IEnumerable<Ecomm_ModuleService> result = db.Read(storedProcName, MakeModule, parameters);

            List<Ecomm_Module> modules = new List<Ecomm_Module>();

            if (result != null)
            {
                foreach(var module in result)
                {
                    var m = new Ecomm_Module
                    {
                        Id = module.ModuleId,
                        Code = module.ModuleCode,
                        Name = module.ModuleName,
                        Description = module.ModuleDescription,
                        SortOrder = module.ModuleSortOrder,
                        Services = new List<Ecomm_Service>()

                    };

                    var selmodule = (from mo in modules where mo.Id == m.Id select mo).FirstOrDefault();

                    var s = new Ecomm_Service
                    {
                        Id = module.ServiceId,
                        Code = module.ServiceCode,
                        Name = module.ServiceName,
                        Description = module.ServiceDescription,
                        SortOrder = module.ServiceSortOrder
                    };

                    if (selmodule == null)
                    {
                        m.Services.Add(s); 
                        modules.Add(m);
                    }
                    else
                    {
                        selmodule.Services.Add(s);
                    }
                }
            }

            if (user.IsAdmin || user.IsSuperAdmin)
            {
                //get all modules
            } 
            else
            {
                //get modules based on user permissions
            }

            return modules;
        }
    }
}
