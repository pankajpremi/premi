using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using JMM.APEC.DAL.EnterpriseLibrary;
using System.Data.SqlTypes;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetGatewayDao : IAssetGatewayDao
    {
        private string _databaseName;
        private Db db;

        public AssetGatewayDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a System_Category object based on DataReader
        static Func<IDataReader, Asset_Gateway> Make = reader =>
           new Asset_Gateway
           {
               Id = reader["gatewayID"].AsId(),
               Portal = new Asset_Portal
               {
                   Id = reader["portalID"].AsId(),
                   Name = reader["portalName"].AsString(),
                   Active = reader["portalIsActive"].AsBool()
               },

               Code = reader["code"].AsString(),
               Name = reader["gatewayName"].AsString(),
               ShortName = reader["gatewayShortName"].AsString(),
               EffectiveEndDate = (reader["EffectiveEndDate"] as DateTime?).GetValueOrDefault(),
               AddressId = reader["AddressId"].AsInt(),

               Status = new System_Status
               {
                   Id = reader["statusid"].AsId(),
                   Code = reader["statusCode"].AsString(),
                   Value = reader["statusValue"].AsString()
               }
               
           };


     
       
        DbParameter[] Take(Asset_Gateway gateway)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inGatewayID", DbType = DbType.Int32, Value = gateway.Id },
                new SqlParameter(){ ParameterName="@inName", DbType = DbType.String, Value = gateway.Name },
                new SqlParameter(){ ParameterName="@inShortName", DbType = DbType.String, Value = gateway.ShortName },
                new SqlParameter(){ ParameterName="@StatusId", DbType = DbType.Int32, Value = gateway.StatusId.HasValue ? gateway.StatusId :null, IsNullable=true },
                new SqlParameter(){ ParameterName="@EffectiveEndDate", DbType = DbType.DateTime, Value = gateway.EffectiveEndDate == DateTime.MinValue?  (Object)DBNull.Value :gateway.EffectiveEndDate},
                new SqlParameter(){ ParameterName="@addressId", DbType = DbType.Int32, Value = gateway.AddressId.HasValue ? gateway.AddressId :null, IsNullable=true },
                new SqlParameter(){ ParameterName="@AppChangeUserId", DbType = DbType.Int32, Value = gateway.AppChangeUserId }
            };

            return parameters;
        }

    
        public List<Asset_Gateway> GetGatewaysForUser(ApplicationSystemUser user)
        {
            List<Asset_Gateway> gatewayList = GetGateway(user.Portal.PortalId, null, null, null, null);

            if (user.IsAdmin || user.IsSuperAdmin)
            {
                //get all modules for this portal

                return gatewayList;
            }
            else
            {
                var results = from g1 in gatewayList
                              join g2 in user.Gateways on g1.Code equals g2.Code
                              select g1;

                return results.ToList();
            }
        }

        public List<Asset_Gateway> GetGateway(int? PortalID, bool? IsPortalActive, int? GatewayId, string GatewayCode, int? GatewayStatusId)
        {
            if(GatewayCode == null || GatewayCode == "")
            {
                GatewayCode = null;
            }

            string storedProcName = "Asset_getGateway";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inPortalID", DbType = DbType.Int32, Value = PortalID.HasValue ? PortalID : null },
                new SqlParameter(){ ParameterName="@inIsPortalActive", DbType = DbType.Boolean, Value = IsPortalActive.HasValue ? IsPortalActive : null },
                new SqlParameter(){ ParameterName="@inGatewayID", DbType = DbType.Int32, Value = GatewayId.HasValue ? GatewayId : null },
                new SqlParameter(){ ParameterName="@inGatewayCode", DbType = DbType.String, Value = GatewayCode },
                new SqlParameter(){ ParameterName="@inGatewayStatusID", DbType = DbType.Int32, Value = GatewayStatusId.HasValue ? GatewayStatusId : null }
            };

            IEnumerable<Asset_Gateway> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for PortalID '{0}', IsPortalActive '{1}', GatewayId '{2}', GatewayCode '{3}', GatewayStatusId '{4}'.", PortalID, IsPortalActive, GatewayId, GatewayCode, GatewayStatusId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        
        public Asset_Gateway InsertGateway(Asset_Gateway gateway)
        {
            string storedProcName = "Asset_saveGateway";
            int gatewayId = db.Insert(storedProcName, Take(gateway));
            if (gateway.Id < 0)
            {
                gateway.Id = -1;
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                gateway.Id = gatewayId;
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + gateway.Id.ToString());
            }

            return gateway;
        }

        public Asset_Gateway UpdateGateway(Asset_Gateway gateway)
        {
            List<Asset_Gateway> gatewaylist =  GetGateway(null, null, gateway.Id, null, null);

            if(gatewaylist != null && gatewaylist.Count > 0)
            {
                foreach(Asset_Gateway g in gatewaylist)
                {
                    gateway.AddressId = g.AddressId == 0 ? null : g.AddressId;

                    string storedProcName = "Asset_saveGateway";

                    int gatewayId = db.Update(storedProcName, Take(gateway));
                    if (gatewayId < 0)
                    {
                        LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to update record");
                        gateway.Id = -1;
                    }
                    else
                    {
                        LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");
                        gateway.Id = gatewayId;
                    }
                   
                }
                return gateway;
            }
            else
            {
                gateway.Id = -1;
                return gateway;
            }
         
        }

        public void DeleteGateway(Asset_Gateway gateway)
        {
            string storedProcName = "Asset_deleteGateway";

            db.Delete(storedProcName, Take(gateway));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return;
        }
             

        private int SaveAddress(Asset_GatewayLocation Location)
        {
            List<Asset_Gateway> glist = null;
            int AddressId = 0;
            int retval = 0;

            glist = GetGateway(null, null, Location.GatewayId, null, null);

            if (glist != null && glist.Count > 0)
            {
                foreach (Asset_Gateway g in glist)
                {
                    if (g.AddressId == null || g.AddressId == 0)
                    {
                        //insert address
                        Asset_Address Address = new Asset_Address();
                        Address.Id = 0;
                        Address.Address1 = Location.Address1;
                        Address.Address2 = Location.Address2;
                        Address.CornerAddress = null;
                        Address.City = Location.City;

                        System_State s = new System_State();
                        s.Code = Location.StateCode;
                        Address.State = s;

                       // Address.StateId = Location.StateId.GetValueOrDefault();
                        Address.CountyId = null;
                        Address.PostalCode = Location.PostalCode;
                        Address.TimeZoneId = null;

                        System_Country c= new System_Country();
                        c.Code = Location.CountryCode;
                        Address.Country = c;

                        //Address.CountryId = Location.CountryId.GetValueOrDefault();
                        Address.Active = true;
                        Address.IsDeleted = false;
                        Address.AppChangeUserId = Location.AppChangeUserId;

                        AssetAddressDao addressDao = new AssetAddressDao(_databaseName);
                        AddressId = addressDao.InsertAddress(Address);
                        
                        //insert to parent table
                        if (AddressId > 0)
                        {
                            g.AddressId = AddressId;
                            g.AppChangeUserId = Location.AppChangeUserId;
                            if(UpdateGateway(g).Id < 0)
                            {
                                retval = -1;
                            }
                        }
                        else
                        {
                            retval =  - 1;
                        }
                    }

                    else
                    {
                        //update address
                        AssetAddressDao addressDao = new AssetAddressDao(_databaseName);
                        List<Asset_Address> AddressList = addressDao.GetAddress(g.AddressId.GetValueOrDefault());
                        if(AddressList != null && AddressList.Count > 0)
                        {
                            foreach(Asset_Address Address in AddressList)
                            {
                                Address.Id = g.AddressId.GetValueOrDefault();                            
                                Address.Address1 = Location.Address1;
                                Address.Address2 = Location.Address2;
                                Address.City = Location.City;

                                System_State s = new System_State();
                                s.Code = Location.StateCode;
                                Address.State = s;

                                //Address.StateId = Location.StateId.GetValueOrDefault();
                                Address.PostalCode = Location.PostalCode;

                                System_Country c = new System_Country();
                                c.Code = Location.CountryCode;
                                Address.Country = c;

                                //Address.CountryId = Location.CountryId.GetValueOrDefault();
                                Address.AppChangeUserId = Location.AppChangeUserId;

                                AddressId = addressDao.UpdateAddress(Address);
                                if(AddressId < 0)
                                {
                                    retval = -1;
                                }
                            }
                        }              
                        
                    }
                }
            }

            return retval;

         }


      

  

        //private int InsertPhone(Asset_GatewayLocation Location)
        //{
        //    AssetPhoneDao phDao = new AssetPhoneDao(_databaseName);
        //    int retval = 0;

        //    if(Location.Phones != null && Location.Phones.Count >0)
        //    {
        //        foreach(Asset_PhoneInfo fone in Location.Phones)
        //        {
        //            Asset_Phone ph = new Asset_Phone();
        //            ph.PhoneId = 0;
        //            ph.Number = fone.Number;
        //            ph.TypeId = fone.TypeId;
        //            ph.IsDeleted =false;
        //            ph.AppChangeUserId = Location.AppChangeUserId;

        //            int phoneId = phDao.InsertPhone(ph);

        //            if (phoneId > 0)
        //            {
        //                //insert to parent table
        //                AssetGatewayPhoneDao gatPhoneDao = new AssetGatewayPhoneDao(_databaseName);

        //                Asset_GatewayPhone gc = new Asset_GatewayPhone();
        //                gc.PhoneId = phoneId;
        //                gc.GatewayId = Location.GatewayId;
        //                gc.AppChangeUserId = Location.AppChangeUserId;

        //                int GatewayPhoneId = gatPhoneDao.InsertGatewayPhone(gc);

        //                if (GatewayPhoneId < 0)
        //                {
        //                    retval= - 1;
        //                }

        //            }
        //            else
        //            {
        //                retval= - 1;
        //            }

        //        }
               
        //    }        

        //    return retval; 

        //}

        private int SavePhone(Asset_GatewayLocation Location)
        {
            //Save phone
            string PhoneIds = null;
            //AssetGatewayPhoneDao gphoneDao = new AssetGatewayPhoneDao(_databaseName);
            AssetPhoneDao phDao = new AssetPhoneDao(_databaseName);
            int retval = 0;
            int PhoneID = 0;

            if(Location.Phones != null && Location.Phones.Count > 0)
            {
                foreach(Asset_PhoneInfo p in Location.Phones)
                {
                    
                        Asset_Phone ph = new Asset_Phone();
                        ph.GatewayId = Location.GatewayId;
                        ph.Number = p.Number;
                        ph.TypeId = p.TypeId;
                        ph.IsDeleted = false;
                        ph.AppChangeUserId = Location.AppChangeUserId;

                       if (p.PhoneId == null || p.PhoneId == 0)
                       {
                            ph.PhoneId = 0;
                            PhoneID = phDao.InsertPhone(ph);                           
                       }
                      else
                       {
                        ph.PhoneId = p.PhoneId.GetValueOrDefault();
                        PhoneID = phDao.UpdatePhone(ph);

                       }

                        if (PhoneID > 0)
                        {
                            PhoneIds += PhoneID + ",";
                        }
                        else
                        {
                            retval = -1;
                        }
                }


                //save phone assignments
                //if(PhoneIds.Length > 0)
                //{ 
                    Asset_PhoneAssignmentEntity phentity = new Asset_PhoneAssignmentEntity();
                    phentity.ObjectCode = "GTEWAY";
                    phentity.EntityId = Location.GatewayId;
                    phentity.PhoneId = PhoneIds;

                    if ( phDao.SavePhoneAssignment(phentity) < 0)
                    {
                        retval = -1;
                    }
                    else
                    {
                        retval = 0;
                    }
                //}

            }
            return retval;
           
        }



        public int SaveGatewayLocation(Asset_GatewayLocation Location)
        {
            int retval = 0;

            if(SaveAddress(Location) < 0)
            {
                retval = -1;
            }
           
            if (SavePhone(Location) < 0)
            {
                retval = -1;
            }

            return retval;

        }        
            
      
        
        public List<Asset_GatewayFacilityList> GetFacilities(ApplicationSystemUser user,int GatewayId, int? FacilityId, int? StatusId,int? TypeId, string GatewayCode, int? GatewayStatusId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            List<Asset_GatewayFacilityList> gfacilities = new List<Asset_GatewayFacilityList>();
             
            //get facilities
            List<Asset_Facility> facilityList;
            AssetFacilityDao facilityDao = new AssetFacilityDao(_databaseName);
            facilityList = facilityDao.GetFacility(user, GatewayId, FacilityId, StatusId, TypeId, GatewayCode, GatewayStatusId, Search, Seed, Limit, Sortcolumn,Sortorder);

            if(facilityList != null && facilityList.Count > 0)
            {
                foreach(Asset_Facility f in facilityList)
                {
                    Asset_GatewayFacilityList gf = new Asset_GatewayFacilityList();

                    gf.Phone = f.PrimaryPhoneNumber;
                    gf.FacilityId = f.Id;
                    gf.FacilityName = f.Name;
                    gf.Status = f.Status.Value;
                    gf.State = f.Address.State.Name;
                    gf.StateId = f.Address.State.Id;
                    gf.StateCode = f.Address.State.Code;
                    gf.CountryCode = f.Address.Country.Code;
                    gf.CountryId = f.Address.Country.Id;
                    gf.CountryName = f.Address.Country.Name;
                    gf.CountyCode = f.Address.County.Code;
                    gf.CountyId = f.Address.County.Id;
                    gf.CountyName = f.Address.County.Name;
                    gf.AddressId = f.AddressId;

                    //get address info
                    if(gf.AddressId != null || gf.AddressId != 0)
                    { 
                        List<Asset_Address> AddressList;
                        AssetAddressDao addressDao = new AssetAddressDao(_databaseName);
                        int AddressId = gf.AddressId.GetValueOrDefault();

                        AddressList = addressDao.GetAddress(AddressId);

                        if (AddressList != null && AddressList.Count > 0)
                        {
                            foreach(Asset_Address gAddress in AddressList)
                            { 
                                gf.City = gAddress.City;
                                gf.State = gAddress.State.Name;                              
                            }
                        }
                     }
                    gfacilities.Add(gf);
                }
            }
            
            if (gfacilities == null || gfacilities.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for GatewayId '{0}'.", GatewayId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), gfacilities.Count().ToString());

            return gfacilities;
        }

        public List<Asset_FacilityDetails> GetFacilityDetails(ApplicationSystemUser user, int GatewayId, int FacilityId)
        {
            List<Asset_FacilityDetails> gfacilities = new List<Asset_FacilityDetails>();

            //get facility
            List<Asset_Facility> facilityList;
            AssetFacilityDao facilityDao = new AssetFacilityDao(_databaseName);
            facilityList = facilityDao.GetFacility(user, GatewayId, FacilityId, null, null, null, null, null, null, null, null, null);

            //if (facilityList != null && facilityList.Count > 0)
            //{
            //    foreach (Asset_Facility f in facilityList)
            //    {
            //        Asset_FacilityDetails gf = new Asset_FacilityDetails();

            //        gf.FacilityId = f.Id;
            //        gf.FacilityName = f.Name;
            //        gf.FacilityAKA = f.AKAName;
            //        gf.TypeId = f.TypeId;
            //        gf.OwnerId = null;
            //        gf.OwnerStatusId = null;
            //        gf.StateId = f.Status.Id;

            //        //get dates if type = fuel
            //        gf.ClosedDate = null;
            //        gf.EffectiveOpsDate = null;
            //        gf.AnticipatedOpsDate = null;
            //        gf.ComplianceMgmtDate = null;

            //        gf.BillingTemplateId = null;

            //        gf.AddressId = f.AddressId;

            //        //get address info
            //        if (gf.AddressId != null)
            //        {
            //            List<Asset_Address> AddressList;
            //            AssetAddressDao addressDao = new AssetAddressDao(_databaseName);
            //            int AddressId = gf.AddressId.GetValueOrDefault();

            //            AddressList = addressDao.GetAddress(AddressId);

            //            if (AddressList != null && AddressList.Count > 0)
            //            {
            //                Asset_Address gAddress = AddressList.SingleOrDefault();
            //                if (gAddress != null)
            //                {
            //                    gf.Address1 = gAddress.Address1;
            //                    gf.Address2 = gAddress.Address2;
            //                    gf.CornerAddress = gAddress.CornerAddress;
            //                    gf.City = gAddress.City;
            //                    gf.StateId = gAddress.StateId.GetValueOrDefault();
            //                    gf.CountryId = gAddress.CountryId.GetValueOrDefault();
            //                    gf.CountyId = gAddress.CountyId.GetValueOrDefault(); //add this to sp select
            //                    gf.PostalCode = gAddress.PostalCode;
            //                    gf.TimeZoneId = gAddress.TimeZoneId.GetValueOrDefault();
            //                    gf.Latitude = gAddress.Latitude.GetValueOrDefault();
            //                    gf.Longitude = gAddress.Longitude.GetValueOrDefault();
            //                }
            //            }
            //        }
            //        gfacilities.Add(gf);
            //    }
            //}

            if (gfacilities == null || gfacilities.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for GatewayId '{0}', FacilityId '{1}'.", GatewayId, FacilityId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), gfacilities.Count().ToString());

            return gfacilities;
        }


    }
}
