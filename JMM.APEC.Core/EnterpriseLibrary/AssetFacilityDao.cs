using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using JMM.APEC.WebAPI.Logging;
using System.Reflection;
using System.Data.Common;
using Microsoft.SqlServer.Server;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetFacilityDao : IAssetFacilityDao
    {
        
        private string _databaseName;
        private Db db;

        public AssetFacilityDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        

        public List<Asset_Facility> GetFacility(ApplicationSystemUser user,int GatewayId, int? FacilityId, int? StatusId, int? TypeId, string GatewayCode, int? GatewayStatusId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            if (GatewayCode == "null" || GatewayCode == "")
            {
                GatewayCode = null;
            }

            string storedProcName = "Asset_getFacility";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inGatewayID", DbType = DbType.Int32, Value = GatewayId },
                new SqlParameter(){ ParameterName="@inFacilityID", DbType = DbType.Int32, Value = FacilityId.HasValue ? FacilityId : null },
                new SqlParameter(){ ParameterName="@inStatusID", DbType = DbType.Int32, Value = StatusId.HasValue ? StatusId : null },
                new SqlParameter(){ ParameterName="@inTypeID", DbType = DbType.Int32, Value = TypeId.HasValue ? TypeId : null },
                new SqlParameter(){ ParameterName="@inGatewayCode", DbType = DbType.String, Value = GatewayCode },
                new SqlParameter(){ ParameterName="@inGatewayStatusID", DbType = DbType.Int32, Value = GatewayStatusId.HasValue ? GatewayStatusId : null },
                new SqlParameter(){ ParameterName="@Search", DbType = DbType.String, Value = Search },
                new SqlParameter(){ ParameterName="@PageNum", DbType = DbType.Int32, Value = Seed.HasValue ? Seed : null },
                new SqlParameter(){ ParameterName="@RecordsPerPage", DbType = DbType.Int32, Value = Limit.HasValue ? Limit : null },
                new SqlParameter(){ ParameterName="@SortCol", DbType = DbType.String, Value = Sortcolumn  },
                new SqlParameter(){ ParameterName="@Sortdir", DbType = DbType.String, Value = Sortorder  }

            };

            IEnumerable<Asset_Facility> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for GatewayId '{0}', FacilityId '{1}', StatusId '{2}', TypeId '{3}', GatewayCode '{4}', GatewayStatusId '{5}'.", GatewayId, FacilityId, StatusId, TypeId,GatewayCode, GatewayStatusId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        public List<Asset_Facility> GetTopFacilities(ApplicationSystemUser user)
        {
            string storedProcName = "Asset_getTopFacilities";

            List<SqlDataRecord> GatewayList = new List<SqlDataRecord>();
            
            if (user != null)
            {
                if(user.Gateways.Count > 0)
                {
                    foreach (ApplicationUserGateway g in user.Gateways)
                    {
                       
                            SqlDataRecord gsdr = new SqlDataRecord(new SqlMetaData[] { new SqlMetaData("GatewayId", SqlDbType.Int) });
                            gsdr.SetInt32(0, Convert.ToInt32(g.PortalGatewayId));
                            GatewayList.Add(gsdr);
                       
                    }
                }
               
            }

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inGatewayID", SqlDbType = SqlDbType.Structured, Value = GatewayList }
            };

            IEnumerable<Asset_Facility> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for User '{0}', gateways '{1}'.", user.UserName, user.Gateways[0].Name);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();          


        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_Facility> Make = reader =>
           new Asset_Facility
           {
               Id = reader["facilityID"].AsId(),
               GatewayId = reader["gatewayid"].AsInt(),
               Name = reader["facilityName"].AsString(),
               AKAName = reader["akaname"].AsString(),
               AddressId = reader["addressID"].AsInt(),
               Address = new Asset_Address
               {
                    PostalCode = reader["akaname"].AsString(),
                    State = new System_State
                    {
                        Id = reader["stateid"].AsInt(),
                        Code = reader["statecode"].AsString(),
                        Name = reader["statename"].AsString()
                    },
                    Country = new System_Country
                    {
                        Id = reader["countryid"].AsInt(),
                        Code = reader["countrycode"].AsString(),
                        Name = reader["countryname"].AsString()
                    },
                    County = new System_County
                    {
                        Id = reader["countyid"].AsInt(),
                        Code = reader["countycode"].AsString(),
                        Name = reader["countyname"].AsString()
                    }

               },
               StatusId = reader["statusID"].AsInt(),

               Status = new System_Status
               {
                   Id = reader["statusID"].AsId(),
                   Code = reader["statusCode"].AsString(),
                   Value = reader["statusValue"].AsString(),
                   Description = reader["statusDesc"].AsString()
               },

               TypeId = reader["typeID"].AsInt(),
              
               Type = new System_Type 
               {
                   TypeId = reader["typeID"].AsInt(),
                   TypeCode = reader["typeCode"].AsString(),
                   TypeName = reader["typeName"].AsString()
               },

               IsDeleted = reader["isDeleted"].AsBool(),
               PrimaryPhoneNumber = reader["primaryPhoneNum"].AsString()
           };


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_Facility> MakeMap = reader =>
           new Asset_Facility
           {
               Id = reader["facilityID"].AsId(),
               GatewayId = reader["gatewayid"].AsInt(),
               Name = reader["facilityName"].AsString(),
                             
               Address = new Asset_Address
               {
                  Address1 = reader["address1"].AsString(),
                  Address2 = reader["address2"].AsString(),
                  City = reader["city"].AsString(),
                  PostalCode = reader["city"].AsString(),
                  StateId = reader["StateID"].AsInt(),
                  CountryId = reader["CountryID"].AsInt(),
                  Latitude = reader["latitude"].AsDouble(),
                  Longitude= reader["longitude"].AsDouble(),

                   State = new System_State
                  {
                      Name = reader["state"].AsString()
                  },

                  Country = new System_Country
                  {
                      Name = reader["country"].AsString()
                  }
                  
               } 
           };



        public int InsertFacility(Asset_Facility facility)
        {
            string storedProcName = "Asset_saveFacility";

            facility.Id = db.Insert(storedProcName, Take(facility));

            if (facility.Id <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + facility.Id.ToString());
            }

            return facility.Id;
        }

        public int UpdateFacility(Asset_Facility facility)
        {
            string storedProcName = "Asset_saveFacility";

            int facilityid = db.Update(storedProcName, Take(facility));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");

            return facilityid;
        }


        public int DeleteFacility(int facilityId)
        {
            string storedProcName = "Asset_deleteFacility";

           int retval =  db.Delete(storedProcName, TakeDelete(facilityId));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return retval;
        }

        DbParameter[] TakeDelete(int facilityId)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityID", DbType = DbType.Int32, Value = facilityId }
            };

            return parameters;
        }

        DbParameter[] Take(Asset_Facility facility)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityID", DbType = DbType.Int32, Value = facility.Id },
                new SqlParameter(){ ParameterName="@GatewayId", DbType = DbType.Int32, Value = facility.GatewayId},
                new SqlParameter(){ ParameterName="@Name", DbType = DbType.String, Value = facility.Name },
                new SqlParameter(){ ParameterName="@AKAName", DbType = DbType.String, Value = facility.AKAName },
                new SqlParameter(){ ParameterName="@AddressId", DbType = DbType.Int32, Value = facility.AddressId},
                new SqlParameter(){ ParameterName="@StatusId", DbType = DbType.Int32, Value = facility.StatusId},
                new SqlParameter(){ ParameterName="@TypeId", DbType = DbType.Int32, Value = facility.TypeId},
                new SqlParameter(){ ParameterName="@IsDeleted", DbType = DbType.Boolean, Value = facility.IsDeleted },
                new SqlParameter(){ ParameterName="@AppChangeUserId", DbType = DbType.Int32, Value = facility.AppChangeUserId }
            };

            return parameters;
        }

        public List<Asset_Contact> GetFacilityContactsByFacilityId(ApplicationSystemUser user,int FacilityId)
        {
            List<Asset_Contact> contactslist = new List<Asset_Contact>();

            //Asset_Contact contact1 = new Asset_Contact();
            //contact1.ContactId = 1;
            //contact1.FirstName = "Eric";
            //contact1.LastName = "Nordstrom";
            //contact1.Title = "Facility Manager";
            //contact1.Email = "enordstrom@gmail.com";
            //contact1.Company = "JMM";
            //contact1.Phone = "847-726-8188";

            //Asset_Contact contact2 = new Asset_Contact();
            //contact2.ContactId = 2;
            //contact2.FirstName = "Ash";
            //contact2.LastName = "Taylor";
            //contact2.Title = "Supervisior";
            //contact2.Email = "aTaylor@gmail.com";
            //contact2.Company = "ABC";
            //contact1.Phone = "847-456-0088";

            //Asset_Contact contact3 = new Asset_Contact();
            //contact3.ContactId = 3;
            //contact3.FirstName = "Luke";
            //contact3.LastName = "Powell";
            //contact3.Title = "Facility Manager";
            //contact3.Email = "lpowell@gmail.com";
            //contact3.Company = "ETS";
            //contact1.Phone = "847-622-7841";

            //Asset_Contact contact4 = new Asset_Contact();
            //contact4.ContactId = 4;
            //contact4.FirstName = "Eve";
            //contact4.LastName = "Levin";
            //contact4.Title = "Facility Operator";
            //contact4.Email = "elevin@gmail.com";
            //contact4.Company = "Orion";
            //contact1.Phone = "847-118-3973";

            //Asset_Contact contact5 = new Asset_Contact();
            //contact5.ContactId = 5;
            //contact5.FirstName = "Augustine";
            //contact5.LastName = "Thomas";
            //contact5.Title = "Facility Manager";
            //contact5.Email = "aThomas@gmail.com";
            //contact5.Company = "Primrose";
            //contact1.Phone = "847-883-1342";


            //contactslist.Add(contact1);
            //contactslist.Add(contact2);
            //contactslist.Add(contact3);
            //contactslist.Add(contact4);
            //contactslist.Add(contact5);

            return contactslist;

        }

        public List<Asset_Facility> GetFacilityLatLong(int FacilityId)
        {
            string storedProcName = "Asset_getFacilityMap";

            List<Asset_Facility> faclist = new List<Asset_Facility>();

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityID", DbType=DbType.Int32, Value = FacilityId }
            };

            IEnumerable<Asset_Facility> result = db.Read(storedProcName, MakeMap, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for facilityId '{0}'.", FacilityId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

            //return faclist;

        }


        public List<Asset_Address> GetFacilityAddress(int FacilityId)
        {
            string storedProcName = "Asset_getAddress";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityID", DbType = DbType.Int32, Value = FacilityId }
            };

            IEnumerable<Asset_Address> result = db.Read(storedProcName, MakeAddress, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for FacilityId '{0}'.", FacilityId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        // creates a Asset_Portal object based on DataReader
        static Func<IDataReader, Asset_Address> MakeAddress = reader =>
           new Asset_Address
           {
               Id = reader["addressid"].AsInt(),
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

               Latitude = reader["latitude"].AsDouble(),
               Longitude = reader["longitude"].AsDouble()
           };


        public int SaveFacilityDetails(Asset_FacilityDetails facdetails)
        {
            int retval = 0;

           if(facdetails.FacilityId == 0 || facdetails.FacilityId == null)
            {
                if(InsertFacilityDetails(facdetails) < 0)
                {
                    retval = -1;
                }
            }
            else
            {
                if(UpdateFacilityDetails(facdetails) < 0)
                {
                    retval = -1;
                }
            }
            return retval;

        }


        public int InsertFacilityDetails(Asset_FacilityDetails f)
        {
                int AddressId = 0;
                int FacilityId = 0;
                int retval = 0;

                AssetAddressDao  addressDao = new AssetAddressDao(_databaseName);

                Asset_Address facaddr = new Asset_Address();

                facaddr.Id = 0;
                facaddr.Address1 = f.Address1;
                facaddr.Address2 = f.Address2;
                facaddr.CornerAddress = f.CornerAddress;
                facaddr.City = f.City;

                System_Country c = new System_Country();
                c.Code = f.CountryCode;
                facaddr.Country = c;

                System_State s = new System_State();
                s.Code = f.StateCode;
                facaddr.State = s;

                System_County cty = new System_County();
                cty.Code = f.CountyCode;
                facaddr.County = cty;

            //facaddr.CountryId = f.CountryId;
            //facaddr.StateId = f.StateId;
            //facaddr.CountyId = f.CountyId;
            facaddr.PostalCode = f.PostalCode;

                System_TimeZone tz = new System_TimeZone();
                tz.Code = f.TimeZoneCode;
                facaddr.TimeZone = tz;

                //facaddr.TimeZoneId = f.TimeZoneId;
                facaddr.Latitude = f.Latitude;
                facaddr.Longitude = f.Longitude;

               //insert address
                AddressId = addressDao.InsertAddress(facaddr);

                if(AddressId < 0)
                {
                   retval = -1;
                }
               
                //insert facility
                Asset_Facility fac = new Asset_Facility();
                AssetFacilityDao facDao = new AssetFacilityDao(_databaseName);

                fac.Id = 0;
                fac.GatewayId = f.GatewayId;
                fac.TypeId = f.TypeId;
                fac.Name = f.FacilityName;
                fac.AKAName = f.FacilityAKA;
                fac.StatusId = f.StatusId;
                if(AddressId > 0)
                {
                    fac.AddressId = AddressId;
                }
                            
                fac.IsDeleted = false;
                fac.AppChangeUserId = f.AppChangeUserId;

                FacilityId = facDao.InsertFacility(fac);

                if(FacilityId < 0)
                {
                    retval = -1;
                }


            Asset_FacilityFuel ff = new Asset_FacilityFuel();

            if(FacilityId > 0)
            {
                ff.FacilityId = FacilityId;
            }

            ff.EffectiveOpsDate = f.EffectiveOpsDate;
            ff.ComplianceMgmtDate = f.ComplianceMgmtDate;
            ff.AnticipatedOpsDate = f.AnticipatedOpsDate;
            ff.ClosedDate = ff.ClosedDate;
            ff.AppChangeUserID = f.AppChangeUserId;

            AssetFacilityFuelDao facFuelDao = new AssetFacilityFuelDao(_databaseName);
            if(facFuelDao.InsertFacilityFuel(ff) < 0)
            {
                retval = -1;
            }

            return retval;
        }

        public int UpdateFacilityDetails(Asset_FacilityDetails f)
        {
            int retval = 0;
            int retaddrId = 0;
            AssetAddressDao addressDao = new AssetAddressDao(_databaseName);

            List<Asset_Facility> faclist = GetFacility(null, f.GatewayId, f.FacilityId, null, null, null, null, null, null, null, null,null);

            if (faclist != null && faclist.Count > 0)
            {

                foreach (Asset_Facility facility in faclist)
                {
                  
                    //chk if address exists
                    if (facility.AddressId == 0 || facility.AddressId == null)
                    {
                        //Insert Address
                        Asset_Address facaddr = new Asset_Address();
                        facaddr.Id = 0;
                        facaddr.Address1 = f.Address1;
                        facaddr.Address2 = f.Address2;
                        facaddr.CornerAddress = f.CornerAddress;
                        facaddr.City = f.City;

                        System_Country c = new System_Country();
                        c.Code = f.CountryCode;
                        facaddr.Country = c;

                        System_State s = new System_State();
                        s.Code = f.StateCode;
                        facaddr.State = s;

                        System_County cty = new System_County();
                        cty.Code = f.CountyCode;
                        facaddr.County = cty;

                        //facaddr.CountryId = f.CountryId;
                        //facaddr.StateId = f.StateId;
                        //facaddr.CountyId = f.CountyId;
                        facaddr.PostalCode = f.PostalCode;

                        System_TimeZone tz = new System_TimeZone();
                        tz.Code = f.TimeZoneCode;
                        facaddr.TimeZone = tz;

                        //facaddr.TimeZoneId = f.TimeZoneId;
                        facaddr.Latitude = f.Latitude;
                        facaddr.Longitude = f.Longitude;
                        facaddr.AppChangeUserId = f.AppChangeUserId;

                        retaddrId = addressDao.InsertAddress(facaddr);

                        if (retaddrId < 0)
                        {
                            retval = -1;
                        }

                  
                    }
                    else
                    {
                        //update Address
                        List<Asset_Address> addresslist = addressDao.GetAddress(facility.AddressId.GetValueOrDefault());

                        if (addresslist != null && addresslist.Count > 0)
                        {
                            foreach (Asset_Address adr in addresslist)
                            {
                                adr.Id = facility.AddressId.GetValueOrDefault();
                                adr.Address1 = f.Address1;
                                adr.Address2 = f.Address2;
                                adr.CornerAddress = f.CornerAddress;
                                adr.City = f.City;

                                System_Country c = new System_Country();
                                c.Code = f.CountryCode;
                                adr.Country = c;

                                System_State s = new System_State();
                                s.Code = f.StateCode;
                                adr.State = s;

                                System_County cty = new System_County();
                                cty.Code = f.CountyCode;
                                adr.County = cty;

                                //adr.CountryId = f.CountryId;
                                //adr.StateId = f.StateId;
                                //adr.CountyId = f.CountyId;
                                adr.PostalCode = f.PostalCode;

                                System_TimeZone tz = new System_TimeZone();
                                tz.Code = f.TimeZoneCode;
                                adr.TimeZone = tz;

                                //adr.TimeZoneId = f.TimeZoneId;
                                adr.Latitude = f.Latitude;
                                adr.Longitude = f.Longitude;
                                adr.AppChangeUserId = f.AppChangeUserId;

                                retaddrId = addressDao.UpdateAddress(adr);
                                if (retaddrId < 0)
                                {
                                    retval = -1;
                                }

                            }
                        }

                    }

                    //update facility info
                    facility.AKAName = f.FacilityAKA;
                    facility.Name = f.FacilityName;
                    facility.StatusId = f.StatusId;
                    facility.TypeId = f.TypeId;
                    facility.AddressId = retaddrId < 0 ? (int?)null : retaddrId;
                    facility.AppChangeUserId = f.AppChangeUserId;
                    if (UpdateFacility(facility) < 0)
                    {
                        retval = -1;
                    }

                    //chk facilityfuel exists
                    AssetFacilityFuelDao ffldao = new AssetFacilityFuelDao(_databaseName);
                    List<Asset_FacilityFuel> fflist = ffldao.GetFacilityFuel(facility.Id);

                    if (fflist != null && fflist.Count > 0)
                    {
                        foreach (Asset_FacilityFuel ff in fflist)
                        {
                            ff.EffectiveOpsDate = f.EffectiveOpsDate;
                            ff.ClosedDate = f.ClosedDate;
                            ff.AnticipatedOpsDate = f.AnticipatedOpsDate;
                            ff.ComplianceMgmtDate = f.ComplianceMgmtDate;
                            ff.AppChangeUserID = f.AppChangeUserId;

                            if (ffldao.UpdateFacilityFuel(ff) < 0)
                            {
                                retval = -1;
                            }

                        }
                    }
                    else //insert facility fuel
                    {
                        Asset_FacilityFuel ffuel = new Asset_FacilityFuel();
                        ffuel.FacilityId = f.FacilityId.GetValueOrDefault();
                        ffuel.BusinessUnit = null;
                        ffuel.District = null;
                        ffuel.HasCarWash = null;
                        ffuel.GasBrand = null;
                        ffuel.Market = null;
                        ffuel.OperatingHours = null;
                        ffuel.ClassOfTrade = null;
                        ffuel.EffectiveOpsDate = f.EffectiveOpsDate;
                        ffuel.ComplianceMgmtDate = f.ComplianceMgmtDate;
                        ffuel.AnticipatedOpsDate = f.AnticipatedOpsDate;
                        ffuel.ClosedDate = f.ClosedDate;
                        ffuel.AppChangeUserID = f.AppChangeUserId;

                        if (ffldao.InsertFacilityFuel(ffuel) < 0)
                        {
                            retval = -1;
                        }
                    }

                  
                }

                return retval;
                 
            }
            else
            {
                return -1;
            }

        }
    }
}
