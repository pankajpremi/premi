using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetContactDao : IAssetContactDao
    {
        private string _databaseName;
        private Db db;

        public AssetContactDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }


        static Func<IDataReader, Asset_GatewayContactList> Make = reader =>
         new Asset_GatewayContactList
         {
             ContactId = reader["contactID"].AsId(),
             GatewayId = reader["gatewayID"].AsInt(),
             TypeId = reader["typeID"].AsInt(),
             Title = reader["title"].AsString(),
             TypeCode = reader["typeCode"].AsString(),
             TypeName = reader["typeName"].AsString(),
             FirstName = reader["firstname"].AsString(),
             LastName = reader["lastname"].AsString(),
             Company = reader["company"].AsString(),
             AddressId = reader["addressId"].AsInt(),
             Active = reader["IsActive"].AsBool(),
             IsAutoAdd = reader["isAutoAdd"].AsBool(),
             AppChangeUserId = reader["appchangeUserId"].AsInt(),
             Email = reader["email"].AsString(),
             Phone = reader["primaryPhoneNumber"].AsString()

         };

        public List<Asset_GatewayContactList> GetContact(int GatewayId, string Contacts, string ObjectCode, int? TypeID, bool? IsActive, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            string storedProcName = "Asset_getContact";


            var intContactList = DbHelpers.MakeParamIntList(Contacts);

            List<SqlDataRecord> ContactList = DbHelpers.MakeParamRecordList(intContactList, "Id");

            var parameters = new[]{
                 new SqlParameter() {ParameterName="@inGatewayID", DbType= DbType.Int32, Value=GatewayId },
                 new SqlParameter(){ ParameterName="@inContactIDs", SqlDbType = SqlDbType.Structured, Value = ContactList },
                 new SqlParameter() {ParameterName="@inObjectCode", DbType= DbType.String, Value=ObjectCode },
                 new SqlParameter() {ParameterName="@inTypeID", DbType= DbType.Int32, Value=TypeID.HasValue? TypeID :null},
                 new SqlParameter() {ParameterName="@inActive", DbType= DbType.Boolean, Value=IsActive.HasValue?IsActive:null },
                 new SqlParameter(){ ParameterName="@Search", DbType = DbType.String, Value = Search },
                 new SqlParameter(){ ParameterName="@PageNum", DbType = DbType.Int32, Value = Seed.HasValue ? Seed : null },
                 new SqlParameter(){ ParameterName="@RecordsPerPage", DbType = DbType.Int32, Value = Limit.HasValue ? Limit : null },
                 new SqlParameter(){ ParameterName="@SortCol", DbType = DbType.String, Value = Sortcolumn  },
                 new SqlParameter(){ ParameterName="@Sortdir", DbType = DbType.String, Value = Sortorder  }
            };

            IEnumerable<Asset_GatewayContactList> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for  GatewayId '{0}', Contacts '{1}',ObjectCode '{2}', TypeID '{3}', IsActive '{4}', Seed '{5}',Limit '{6}', Sortcolumn '{7}', Sortorder '{8}'.", GatewayId, Contacts, ObjectCode, TypeID, IsActive, Search, Seed, Limit, Sortcolumn, Sortorder);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }

        System.Data.Common.DbParameter[] Take(Asset_Contact Contact)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inContactID", DbType = DbType.Int32, Value = Contact.ContactId },
                new SqlParameter(){ ParameterName="@gatewayID", DbType = DbType.Int32, Value = Contact.GatewayId },
                new SqlParameter(){ ParameterName="@objectCode", DbType = DbType.String, Value = Contact.ObjectCode },
                new SqlParameter(){ ParameterName="@typeID", DbType = DbType.Int32, Value = Contact.TypeId.HasValue ? Contact.TypeId : null },
                new SqlParameter(){ ParameterName="@titleId", DbType = DbType.String, Value = Contact.TitleId.HasValue ? Contact.TitleId : null },
                new SqlParameter(){ ParameterName="@firstName", DbType = DbType.String, Value = Contact.FirstName },
                new SqlParameter(){ ParameterName="@lastName", DbType = DbType.String, Value = Contact.LastName },
                new SqlParameter(){ ParameterName="@company", DbType = DbType.String, Value = Contact.Company},
                new SqlParameter(){ ParameterName="@addressID", DbType = DbType.Int32, Value = Contact.AddressId.HasValue ? Contact.AddressId : null },
                new SqlParameter(){ ParameterName="@email", DbType = DbType.String, Value = Contact.Email },
                new SqlParameter(){ ParameterName="@isActive", DbType = DbType.Boolean, Value = Contact.Active },
                new SqlParameter(){ ParameterName="@isAutoAdd", DbType = DbType.Boolean, Value = Contact.IsAutoAdd },
                new SqlParameter(){ ParameterName="@isDeleted", DbType = DbType.Boolean, Value = Contact.IsDeleted },
                new SqlParameter(){ ParameterName="@appChangeUserID", DbType = DbType.Int32, Value = Contact.AppChangeUserId}
            };

            return parameters;
        }

        public Asset_Contact InsertContact(Asset_Contact Contact)
        {
            string storedProcName = "Asset_saveContact";


            int contactId = db.Insert(storedProcName, Take(Contact));

            if (contactId <= 0)
            {
                Contact.ContactId = -1;
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                Contact.ContactId = contactId;
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + contactId.ToString());
            }

            return Contact;
        }


        public Asset_Contact UpdateContact(Asset_Contact Contact)
        {
            string storedProcName = "Asset_saveContact";


            int contactId = db.Update(storedProcName, Take(Contact));


            if (contactId <= 0)
            {
                Contact.ContactId = -1;
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to update record");
            }
            else
            {
                Contact.ContactId = contactId;
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully Id = " + contactId.ToString());
            }

            return Contact;
        }


        public int DeleteContact(int ContactId, int AppChangeUserId)
        {
            string storedProcName = "Asset_deleteContact";

            int retval = db.Delete(storedProcName, TakeDelete(ContactId, AppChangeUserId));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return retval;
        }

        DbParameter[] TakeDelete(int ContactId, int AppChangeUserId)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inContactID", DbType = DbType.Int32, Value = ContactId },
                new SqlParameter(){ ParameterName="@inAppChangeUserID", DbType = DbType.Int32, Value = AppChangeUserId }
            };

            return parameters;
        }




        public List<Asset_ContactAssignment> GetContactAssignment(string ObjectCode, string EntityId, string ContactId, string TypeId)
        {

            string storedProcName = "Asset_getContactAssignment";

            var intEntityList = DbHelpers.MakeParamIntList(EntityId);
            var intContactList = DbHelpers.MakeParamIntList(ContactId);
            var intTypeList = DbHelpers.MakeParamIntList(TypeId);


            List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

            List<SqlDataRecord> ContactList = DbHelpers.MakeParamRecordList(intContactList, "Id");

            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
                new SqlParameter(){ ParameterName="@inEntityIDs", SqlDbType = SqlDbType.Structured, Value = EntityList },
                new SqlParameter(){ ParameterName="@inContactIDs",SqlDbType = SqlDbType.Structured, Value = ContactList },
                new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList }

            };

            IEnumerable<Asset_ContactAssignment> result = db.Read(storedProcName, MakeContactAssignment, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for ObjectCode '{0}' EntityId '{1}' ContactId '{2}' TypeId '{3}'  .", ObjectCode, EntityId, ContactId, TypeId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_ContactAssignment> MakeContactAssignment = reader =>
           new Asset_ContactAssignment
           {
               ContactAssignmentId = reader["contactAssignmentID"].AsId(),
               Contact = new Asset_Contact
               {
                   ContactId = reader["contactID"].AsInt(),
                   TypeId = reader["typeid"].AsInt(),
                   TitleId = reader["titleId"].AsInt(),
                   FirstName = reader["firstName"].AsString(),
                   LastName = reader["lastName"].AsString(),
                   AddressId = reader["addressID"].AsInt(),
                   Email = reader["email"].AsString(),
                   Active = reader["isActive"].AsBool(),
               },

               ObjectId = reader["objectid"].AsInt(),
               EntityId = reader["entityID"].AsInt()
           };


        public int SaveContactAssignmentByContact(Asset_ContactAssignmentByContact obj)
        {
            string storedProcName = "Asset_saveContactAssignmentByContact";

            object retval = 0;

            retval = db.Scalar(storedProcName, TakeContactAssignment(obj));

            if (retval.AsInt() < 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation failed");
                return -1;
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation succeeded");
                return 0;
            }


        }


        System.Data.Common.DbParameter[] TakeContactAssignment(Asset_ContactAssignmentByContact obj)
        {
            var intEntityList = DbHelpers.MakeParamIntList(obj.EntityId);

            List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = obj.ObjectCode },
                new SqlParameter(){ ParameterName="@inContactID", DbType = DbType.Int32, Value = obj.ContactId },
                new SqlParameter(){ ParameterName="@inEntityIDs", SqlDbType = SqlDbType.Structured, Value = EntityList}

           };

            return parameters;
        }


       public  Asset_ContactInformation SaveContactDetails(ApplicationSystemUser user, Asset_ContactInformation contactdetails)
        {
            Asset_ContactInformation c = null;
            if (contactdetails.ContactId == 0 || contactdetails.ContactId == null)
            {
                c = InsertContactDetails(user, contactdetails);

            }
            else
            {
                c = UpdatetContactDetails(user, contactdetails);

            }
            return c;

        }


        public Asset_ContactInformation InsertContactDetails(ApplicationSystemUser user, Asset_ContactInformation c)
        {
            int AddressId = 0;

            AssetAddressDao addressDao = new AssetAddressDao(_databaseName);

            Asset_Address contactaddr = new Asset_Address();

            contactaddr.Id = 0;
            contactaddr.Address1 = c.Address1;
            contactaddr.Address2 = c.Address2;

            contactaddr.City = c.City;

            System_Country ct = new System_Country();
            ct.Code = c.CountryCode;
            contactaddr.Country = ct;

            System_State s = new System_State();
            s.Code = c.StateCode;
            contactaddr.State = s;

            contactaddr.PostalCode = c.PostalCode;


            //insert address
            AddressId = addressDao.InsertAddress(contactaddr);

            //insert contact
            Asset_Contact contact = new Asset_Contact();

            contact.ContactId = 0;
            contact.GatewayId = c.GatewayId;
            contact.TypeId = c.TypeId;
            contact.TitleId = c.TitleId;
            contact.ObjectCode = c.ObjectCode;
            contact.FirstName = c.FirstName;
            contact.LastName = c.LastName;
            contact.Company = c.Company;

            contact.Email = c.Email;


            if (AddressId < 0)
            {
                c.AddressId = -1;
            }
            else
            {
                c.AddressId = AddressId;
                contact.AddressId = AddressId;
            }

            contact.IsAutoAdd = c.IsAutoAdd.GetValueOrDefault();
            contact.IsDeleted = false;
            contact.AppChangeUserId = user.Id;

            Asset_Contact cont = InsertContact(contact);

            if (cont != null && cont.ContactId > 0)
            {
                c.ContactId = cont.ContactId;
                //phone list
                SavePhoneAssignment(user, c);
                // facility list 
                SaveFacilityAssignment(user, c);
            }
            else
            {
                c.ContactId = -1;
            }

            return c;
        }


        private int SavePhoneAssignment(ApplicationSystemUser user, Asset_ContactInformation coninfo)
        {
            //Save phone
            string PhoneIds = null;
            AssetPhoneDao phDao = new AssetPhoneDao(_databaseName);
            int retval = 0;
            int PhoneID = 0;

            if (coninfo.Phonelist != null && coninfo.Phonelist.Count > 0)
            {
                foreach (Asset_PhoneInfo p in coninfo.Phonelist)
                {

                    Asset_Phone ph = new Asset_Phone();
                    ph.GatewayId = coninfo.GatewayId;
                    ph.Number = p.Number;
                    ph.TypeId = p.TypeId;
                    ph.IsDeleted = false;
                    ph.AppChangeUserId = user.Id;

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


                Asset_PhoneAssignmentEntity phentity = new Asset_PhoneAssignmentEntity();
                phentity.ObjectCode = "CONTCT";
                phentity.EntityId = coninfo.ContactId.GetValueOrDefault();
                phentity.PhoneId = PhoneIds;

                if (phDao.SavePhoneAssignment(phentity) < 0)
                {
                    retval = -1;
                }
                else
                {
                    retval = 0;
                }


            }
            return retval;

        }



        private int SaveFacilityAssignment(ApplicationSystemUser user, Asset_ContactInformation coninfo)
        {

            int retval = 0;
            string FacilityIds = null;

            if (coninfo.Facilitylist != null && coninfo.Facilitylist.Count > 0)
            {
                foreach (Asset_Facility f in coninfo.Facilitylist)
                {
                    FacilityIds += f.Id + ";";
                }


                Asset_ContactAssignmentByContact obj = new Asset_ContactAssignmentByContact();
                obj.ContactId = coninfo.ContactId.GetValueOrDefault();
                obj.EntityId = FacilityIds;
                obj.ObjectCode = "FACLTY";

                if (SaveContactAssignmentByContact(obj) < 0)
                {
                    retval = -1;
                }
                else
                {
                    retval = 0;
                }


            }
            return retval;

        }


        public Asset_ContactInformation UpdatetContactDetails(ApplicationSystemUser user, Asset_ContactInformation c)
        {
            int retval = 0;
            int retaddrId = 0;
            AssetAddressDao addressDao = new AssetAddressDao(_databaseName);

            List<Asset_GatewayContactList> contactlist = GetContact(c.GatewayId, c.ContactId.ToString(), "GTEWAY", null, null, null, null, null, null, null);

            if (contactlist != null && contactlist.Count > 0)
            {

                foreach (Asset_GatewayContactList con in contactlist)
                {

                    //chk if address exists
                    if (con.AddressId == 0 || con.AddressId == null)
                    {
                        Asset_Address contactaddr = new Asset_Address();

                        contactaddr.Id = 0;
                        contactaddr.Address1 = c.Address1;
                        contactaddr.Address2 = c.Address2;

                        contactaddr.City = c.City;

                        System_Country ct = new System_Country();
                        ct.Code = c.CountryCode;
                        contactaddr.Country = ct;

                        System_State s = new System_State();
                        s.Code = c.StateCode;
                        contactaddr.State = s;

                        contactaddr.PostalCode = c.PostalCode;


                        //insert address
                        retaddrId = addressDao.InsertAddress(contactaddr);

                        if (retaddrId < 0)
                        {
                            c.AddressId = -1;
                        }
                        else
                        {
                            c.AddressId = retaddrId;
                        }

                    }
                    else
                    {
                        //update Address
                        List<Asset_Address> addresslist = addressDao.GetAddress(c.AddressId.GetValueOrDefault());

                        if (addresslist != null && addresslist.Count > 0)
                        {
                            foreach (Asset_Address adr in addresslist)
                            {
                                adr.Id = c.AddressId.GetValueOrDefault();
                                adr.Address1 = c.Address1;
                                adr.Address2 = c.Address2;
                                adr.City = c.City;

                                System_Country ct = new System_Country();
                                ct.Code = c.CountryCode;
                                adr.Country = ct;

                                System_State s = new System_State();
                                s.Code = c.StateCode;
                                adr.State = s;

                                adr.PostalCode = c.PostalCode;

                                adr.AppChangeUserId = user.Id;

                                retaddrId = addressDao.UpdateAddress(adr);

                                if (retaddrId < 0)
                                {
                                    c.AddressId = -1;
                                }
                                else
                                {
                                    c.AddressId = retaddrId;
                                }

                            }
                        }

                    }

                    //update contact info

                    Asset_Contact cnt = new Asset_Contact();
                    cnt.ContactId = con.ContactId;
                    cnt.GatewayId = con.GatewayId;
                    cnt.ObjectCode = c.ObjectCode;
                    cnt.TypeId = c.TypeId;
                    cnt.TitleId = c.TitleId;
                    cnt.Company = c.Company;
                    cnt.FirstName = c.FirstName;
                    cnt.LastName = c.LastName;
                    cnt.Email = c.Email;
                    cnt.IsAutoAdd = c.IsAutoAdd.GetValueOrDefault();
                    cnt.AppChangeUserId = user.Id;
                    cnt.AddressId = c.AddressId;

                    cnt = UpdateContact(cnt);

                    if (cnt.ContactId > 0)
                    {
                        c.ContactId = cnt.ContactId;
                        //phone list
                        SavePhoneAssignment(user, c);
                        // facility list 
                        SaveFacilityAssignment(user, c);
                    }
                    else
                    {
                        c.ContactId = -1;
                    }


                }


            }
            return c;
        }




    }
}

