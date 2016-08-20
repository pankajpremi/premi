using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using JMM.APEC.DAL.EnterpriseLibrary;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetAddressDao : IAssetAddressDao
    {
        private string _databaseName;
        private Db db;

        public AssetAddressDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Portal object based on DataReader
        static Func<IDataReader, Asset_Address> Make = reader =>
           new Asset_Address
           {
               Address1 = reader["address1"].AsString(),
               Address2 = reader["address2"].AsString(),
               CornerAddress = reader["CornerAddress"].AsString(),
               City = reader["city"].AsString(),
               StateId = reader["stateid"].AsInt(),

               State = new System_State
               {
                   Code = reader["stateCode"].AsString(),
                   Name = reader["stateName"].AsString(),
               },

               CountryId = reader["CountryID"].AsInt(),

               Country = new System_Country
               {
                   Code = reader["countryCode"].AsString(),
                   Name = reader["countryName"].AsString(),
               },

               PostalCode = reader["PostalCode"].AsString(),
               TimeZoneId = reader["TimeZoneID"].AsInt(),

               TimeZone = new System_TimeZone
               {
                   Code = reader["timeZoneCode"].AsString(),
                   Name = reader["timeZoneName"].AsString(),
                   GMT = reader["gmt"].AsString(),
                   Offset = reader["gmtOffSet"].AsInt(),
               },

               Latitude = reader["Latitude"].AsDouble(),
               Longitude = reader["Longitude"].AsDouble(),

               CountyId = reader["CountyID"].AsInt(),

               County = new System_County
               {
                  Name = reader["countyName"].AsString(),
                  Code = reader["countyCode"].AsString()
               }
           };



        public List<Asset_Address> GetAddress(int AddressId)
        {
            string storedProcName = "Asset_getAddress";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAddressID", DbType = DbType.Int32, Value = AddressId }               
            };

            IEnumerable<Asset_Address> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for AddressId '{0}'.", AddressId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        System.Data.Common.DbParameter[] Take(Asset_Address Address)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAddressID", DbType = DbType.Int32, Value = Address.Id },
                new SqlParameter(){ ParameterName="@address1", DbType = DbType.String, Value = Address.Address1 },
                new SqlParameter(){ ParameterName="@address2", DbType = DbType.String, Value = Address.Address2 },
                new SqlParameter(){ ParameterName="@cornerAddress", DbType = DbType.String, Value = Address.CornerAddress },
                new SqlParameter(){ ParameterName="@city", DbType = DbType.String, Value = Address.City},               
                //new SqlParameter(){ ParameterName="@stateID", DbType = DbType.Int32, Value = Address.StateId == 0? null:Address.StateId },     
                new SqlParameter(){ ParameterName="@stateCode", DbType = DbType.String, Value = Address.State.Code == "" ? null:Address.State.Code },
               // new SqlParameter(){ ParameterName="@countyID", DbType = DbType.Int32, Value = Address.CountyId== 0? null:Address.CountyId },
                new SqlParameter(){ ParameterName="@CountyCode", DbType = DbType.String, Value = Address.County.Code == "" ? null:Address.County.Code },
                new SqlParameter(){ ParameterName="@postalCode", DbType = DbType.String, Value = Address.PostalCode },
                //new SqlParameter(){ ParameterName="@timezoneID", DbType = DbType.Int32, Value = Address.TimeZoneId== 0? null:Address.TimeZoneId},
                //new SqlParameter(){ ParameterName="@countryID", DbType = DbType.Int32, Value = Address.CountryId  == 0? null:Address.CountryId},  
                new SqlParameter(){ ParameterName="@timezoneCode", DbType = DbType.String, Value = Address.TimeZone.Code == "" ? null:Address.TimeZone.Code },
                new SqlParameter(){ ParameterName="@countryCode", DbType = DbType.String, Value = Address.Country.Code== "" ? null:Address.Country.Code },
                new SqlParameter(){ ParameterName="@latitude", DbType = DbType.Double, Value = Address.Latitude },
                new SqlParameter(){ ParameterName="@longitude", DbType = DbType.Double, Value = Address.Longitude },
                new SqlParameter(){ ParameterName="@isActive", DbType = DbType.Boolean, Value = Address.Active },
                new SqlParameter(){ ParameterName="@isDeleted", DbType = DbType.Boolean, Value = Address.IsDeleted },
                new SqlParameter(){ ParameterName="@appChangeUserID", DbType = DbType.Int32, Value = Address.AppChangeUserId}
            };

            return parameters;
        }

        public int InsertAddress(Asset_Address Address)
        {
            string storedProcName = "Asset_saveAddress";


            Address.Id = db.Insert(storedProcName, Take(Address));

            if (Address.Id <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + Address.Id.ToString());
            }

            return Address.Id;
        }


        public int UpdateAddress(Asset_Address Address)
        {
            string storedProcName = "Asset_saveAddress";


            int AddressId = db.Update(storedProcName, Take(Address));
            LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");
           
            return AddressId;
        }

    }
}
