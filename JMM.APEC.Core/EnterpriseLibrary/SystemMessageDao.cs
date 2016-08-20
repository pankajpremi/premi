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
    public class SystemMessageDao : ISystemMessageDao
    {
        private string _databaseName;
        private Db db;

        public SystemMessageDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, System_MessageList> Make = reader =>
           new System_MessageList
           {
               MessageId = reader["messageid"].AsId(),
               TypeId = reader["typeID"].AsInt(),
               TypeCode = reader["typeCode"].AsString(),
               TypeName = reader["typeName"].AsString(),
               Message = reader["message"].AsString(),
               Subject = reader["subject"].AsString(),
               BeginDateTime = reader["begindatetime"].AsDateTime(),
               EndDateTime = reader["enddatetime"].AsDateTime(),
               IsDismissible = reader["IsDissmissible"].AsBool(),
               GatewayCount = reader["gatewayCount"].AsInt(),
               UserFromId = reader["userFromID"].AsInt(),
               UserToId = reader["userToID"].AsInt(),
               IsDeleted = reader["isDeleted"].AsBool()

           };

        
        public List<System_MessageList> GetMessage(int? MessageId,string ObjectCode, string TypeCode, string UserToId, string UserFromId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            string storedProcName = "System_getMessageList";

            //var intTypeList = DbHelpers.MakeParamIntList(TypeId);
            var intUserToList = DbHelpers.MakeParamIntList(UserToId);
            var intUserFromList = DbHelpers.MakeParamIntList(UserFromId);

           // List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

            List<SqlDataRecord> UserToList = DbHelpers.MakeParamRecordList(intUserToList, "Id");

            List<SqlDataRecord> UserFromList = DbHelpers.MakeParamRecordList(intUserFromList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@messageID", DbType = DbType.Int32, Value = MessageId.HasValue ? MessageId : null },
                //new SqlParameter(){ ParameterName="@typeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList},
                new SqlParameter() {ParameterName="@inObjectCode", DbType= DbType.String, Value=ObjectCode },
                new SqlParameter() {ParameterName="@typeCode", DbType= DbType.String, Value=TypeCode },
                new SqlParameter(){ ParameterName="@userToIDs",SqlDbType = SqlDbType.Structured, Value = UserToList },
                new SqlParameter(){ ParameterName="@userFromIDs", SqlDbType = SqlDbType.Structured, Value = UserFromList },               
                new SqlParameter() {ParameterName="@Search", DbType= DbType.String, Value=Search },
                new SqlParameter(){ ParameterName="@PageNum", DbType = DbType.Int32, Value = Seed.HasValue ? Seed:null },
                new SqlParameter(){ ParameterName="@RecordsPerPage", DbType = DbType.Int32, Value = Limit.HasValue ? Limit:null },
                new SqlParameter() {ParameterName="@SortCol", DbType= DbType.String, Value=Sortcolumn },
                new SqlParameter() {ParameterName="@Sortdir", DbType= DbType.String, Value=Sortorder }
            };

             IEnumerable<System_MessageList> result = db.Read(storedProcName, Make, parameters);           

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found");
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            List<System_MessageList> output = result.ToList();

            foreach (System_MessageList item in output)
            {
                if(item.BeginDateTime > DateTime.Now)
                {
                    item.Status = "Scheduled";
                }
                else if(item.EndDateTime < DateTime.Now)
                {
                    item.Status = "Expired";
                }
                else if(item.BeginDateTime < DateTime.Now && item.EndDateTime > DateTime.Now)
                {
                    item.Status = "Active";
                }
                else
                {
                    //Expired
                    item.Status = "Expired";
                }
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), output.Count().ToString());

            return output;
        }

        DbParameter[] Take(int UserId,System_MessageAssignmentList msg)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inMessageId", DbType = DbType.Int32, Value = msg.MessageId == 0? null :  msg.MessageId },
                new SqlParameter(){ ParameterName="@objectCode", DbType = DbType.String, Value = msg.ObjectCode },
                new SqlParameter(){ ParameterName="@typeCode", DbType = DbType.String, Value = msg.TypeCode },
                new SqlParameter(){ ParameterName="@userToId", DbType = DbType.Int32, Value = msg.UserToId== 0? null:msg.UserToId},
                new SqlParameter(){ ParameterName="@userFromId", DbType = DbType.Int32, Value = UserId },
                new SqlParameter(){ ParameterName="@begindatetime", DbType = DbType.DateTime, Value = msg.BeginDateTime } ,
                new SqlParameter(){ ParameterName="@enddatetime", DbType = DbType.DateTime, Value = msg.EndDateTime == DateTime.MinValue?  (Object)DBNull.Value :msg.EndDateTime},
                new SqlParameter(){ ParameterName="@subject", DbType = DbType.String, Value =msg.Subject },
                new SqlParameter(){ ParameterName="@message", DbType = DbType.String, Value = msg.Message } ,
                new SqlParameter(){ ParameterName="@isDismissible", DbType = DbType.Boolean, Value = msg.IsDismissible }

            };

            return parameters;
        }

        DbParameter[] Take(System_Message msg)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inMessageId", DbType = DbType.Int32, Value = msg.MessageId },
                new SqlParameter(){ ParameterName="@objectCode", DbType = DbType.String, Value = msg.ObjectCode },
                new SqlParameter(){ ParameterName="@typeCode", DbType = DbType.String, Value = msg.TypeCode },
                new SqlParameter(){ ParameterName="@userToId", DbType = DbType.Int32, Value = msg.UserToId== 0? null:msg.UserToId},
                new SqlParameter(){ ParameterName="@userFromId", DbType = DbType.Int32, Value = msg.UserFromId },
                new SqlParameter(){ ParameterName="@begindatetime", DbType = DbType.DateTime, Value = msg.BeginDateTime } ,
                new SqlParameter(){ ParameterName="@enddatetime", DbType = DbType.DateTime, Value = msg.EndDateTime == DateTime.MinValue?  (Object)DBNull.Value :msg.EndDateTime},
                new SqlParameter(){ ParameterName="@subject", DbType = DbType.String, Value =msg.Subject },
                new SqlParameter(){ ParameterName="@message", DbType = DbType.String, Value = msg.Message } ,
                new SqlParameter(){ ParameterName="@isDismissible", DbType = DbType.Boolean, Value = msg.IsDismissible }

            };

            return parameters;
        }

        DbParameter[] TakeDelete(int msgId)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inMessageId", DbType = DbType.Int32, Value = msgId }
              
            };

            return parameters;
        }

        public int DeleteMessage(int MessageId)
        {
            string storedProcName = "System_deleteMessage";

            System_MessageAssignment MessageAssignments = new System_MessageAssignment();
            MessageAssignments.MessageId = MessageId;

            int retval = db.Delete(storedProcName, TakeDelete(MessageId));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return retval;
        }


        public System_MessageAssignmentList InsertMessageAssignment(ApplicationSystemUser user,System_MessageAssignmentList MessageAssignments)
        {
           
            string storedProcName = "System_saveMessage";


            int MessageId = db.Insert(storedProcName, Take(user.Id, MessageAssignments));

            if (MessageId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record.");
                MessageAssignments.MessageId = -1;
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + MessageId.ToString());
                MessageAssignments.MessageId = MessageId;
            }

            if(MessageId > 0)
            {

                //save gateway assignments 
                int MessageAssignmentId = 0;
                MessageAssignmentId = SaveMsgAssignment(MessageId, MessageAssignments);

                if (MessageAssignmentId < 0)
                {
                    MessageAssignments.MessageAssignmentId = -1;
                }
                else
                {
                    MessageAssignments.MessageAssignmentId = MessageAssignmentId;
                }
           }
            return MessageAssignments;        

        }

        public System_Message InsertUserMessage(ApplicationSystemUser user, int ToUserId, string Subject, string Message)
        {
            string storedProcName = "System_saveMessage";

            System_Message userMessage = new System_Message();
            userMessage.UserFromId = user.Id;
            userMessage.UserToId = ToUserId;
            userMessage.IsDismissible = true;
            userMessage.IsDeleted = false;
            userMessage.BeginDateTime = DateTime.UtcNow;
            userMessage.ObjectCode = "USER";
            userMessage.TypeCode = "USRMSG";
            userMessage.Subject = Subject;
            userMessage.Message = Message;

            int MessageId = db.Insert(storedProcName, Take(userMessage));

            if (MessageId > 0)
            {
                userMessage.MessageId = MessageId;
            }
            else
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
                userMessage.MessageId = -1;
            }

            return userMessage;
        }

        public System_MessageAssignmentList UpdateMessageAssignment(ApplicationSystemUser user,System_MessageAssignmentList MessageAssignments)
        {
            string storedProcName = "System_saveMessage";


            int MessageId = db.Update(storedProcName, Take(user.Id,MessageAssignments));

            if (MessageId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to update record");
                MessageAssignments.MessageId = -1;
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully Id = " + MessageId.ToString());
                MessageAssignments.MessageId = MessageId;
            }

            if (MessageId > 0)
            {

                int MessageAssignmentId = 0;
                MessageAssignmentId = SaveMsgAssignment(MessageId, MessageAssignments);

                if (MessageAssignmentId < 0)
                {
                    MessageAssignments.MessageAssignmentId = -1;
                }
                else
                {
                    MessageAssignments.MessageAssignmentId = MessageAssignmentId;
                }
            }
            return MessageAssignments;

        }

        private int SaveMsgAssignment(int MessageId, System_MessageAssignmentList MessageAssignments)
        {

            object retval = 0;
            string gatewayids = null;

            if (MessageAssignments.gatewaylist != null && MessageAssignments.gatewaylist.Count > 0)
            {
                foreach (Asset_GatewayInfo g in MessageAssignments.gatewaylist)
                {

                    gatewayids += g.GatewayId + ",";

                }
            }

            

                string storedProcName = "System_saveMessageAssignment";

                SystemMessageAssignmentEntity obj = new SystemMessageAssignmentEntity();
                obj.EntityId = gatewayids;
                obj.ObjectCode = MessageAssignments.ObjectCode;
                obj.MessageId = MessageId;

                    retval = db.Scalar(storedProcName, TakeMessageAssignment(obj));

                    if (retval.AsInt() < 0)
                    {
                        LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation failed");
                        return retval.AsInt();
            }
                    else
                    {
                        LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation succeeded");
                        return retval.AsInt();
            }     

          
        }


        //private int UpdateMessage(System_MessageAssignmentList Message)
        //{
        //    string storedProcName = "System_saveMessage";
        //    int retval = 0;

                ////Get message
                //List<System_MessageList> msglist = GetMessage(Message.MessageId, null, null, null, null, null, null, null, null);

                //if (msglist != null && msglist.Count > 0)
                //{
                //    foreach (System_MessageList mg in msglist)
                //    {
                //        System_Message m = new System_Message();

                //        m.MessageId = Message.MessageId;
                //        m.ObjectCode = Message.ObjectCode;
                //        m.TypeCode = Message.TypeCode;
                //        m.UserFromId = Message.UserFromId;
                //        m.BeginDateTime = Message.BeginDateTime;
                //        m.EndDateTime = Message.EndDateTime == DateTime.MinValue? Message.EndDateTime: null;
                //        m.Subject = Message.Subject;
                //        m.Message = Message.Message;
                //        m.IsDismissible = Message.IsDismissible;

                //        int MessageId = db.Update(storedProcName, Take(m));

                //        if (MessageId < 0)
                //        {
                //            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to update record");

                //            retval = -1;
                //        }
                //        else
                //        {
                //            //check if messagegateways exists
                //            SystemMessageAssignmentDao mgdao = new SystemMessageAssignmentDao(_databaseName);
                //            List<System_MessageAssignment> mgtlist =  mgdao.GetMessageGatewayList(MessageId);

                //            if (mgtlist != null && mgtlist.Count > 0)
                //            {
                //                if (DeleteExistingMessageGateway(MessageId) < 0)
                //                {
                //                    retval = -1;
                //                }
                //            }  

                //            //insert message gateway
                //            if (Message.gatewaylist != null && Message.gatewaylist.Count > 0)
                //            {
                //                foreach (Asset_GatewayInfo g in Message.gatewaylist)
                //                {
                //                    System_MessageAssignment mgateway = new System_MessageAssignment();
                //                    mgateway.MessageId = MessageId;
                //                    //mgateway.GatewayId = g.GatewayId;

                //                    if (mgdao.InsertMessageGateway(mgateway) < 0)
                //                    {
                //                        retval = -1;
                //                    }
                //                }

                //            }

                //        }
                //    }

                //}   
                //    return retval;
                //}




        public List<System_MessageAssignment> GetMessageAssignment(string ObjectCode, string EntityId, string MessageId, string TypeId)
        {
            string storedProcName = "System_getMessageAssignment";

            var intEntityList = DbHelpers.MakeParamIntList(EntityId);
            var intMessageList = DbHelpers.MakeParamIntList(MessageId);
            var intTypeList = DbHelpers.MakeParamIntList(TypeId);


            List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

            List<SqlDataRecord> MessageList = DbHelpers.MakeParamRecordList(intMessageList, "Id");

            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
                new SqlParameter(){ ParameterName="@inEntityIDs", SqlDbType = SqlDbType.Structured, Value = EntityList },
                new SqlParameter(){ ParameterName="@inMessageIDs",SqlDbType = SqlDbType.Structured, Value = MessageList },
                new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList }

            };

            IEnumerable<System_MessageAssignment> result = db.Read(storedProcName, MakeMessageAssignment, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for ObjectCode '{0}' EntityId '{1}' MessageId '{2}' TypeId '{3}'  .", ObjectCode, EntityId, MessageId, TypeId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }



        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, System_MessageAssignment> MakeMessageAssignment = reader =>
           new System_MessageAssignment
           {
               Id = reader["messageAssignmentID"].AsId(),             
               MessageId = reader["messageID"].AsInt(),                
               EntityId = reader["entityID"].AsInt(),
              
           };


        System.Data.Common.DbParameter[] TakeMessageAssignment(SystemMessageAssignmentEntity obj)
        {
            var intGatewayList = DbHelpers.MakeParamIntList(obj.EntityId);


            List<SqlDataRecord> GatewayList = DbHelpers.MakeParamRecordList(intGatewayList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inMessageID", DbType =DbType.Int32, Value = obj.MessageId},
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = obj.ObjectCode },
                new SqlParameter(){ ParameterName="@inEntityIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList }                   

           };

            return parameters;
        }
    }
}




