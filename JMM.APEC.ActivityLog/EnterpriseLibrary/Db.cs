using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog.EnterpriseLibrary
{
    public class Db
    {
        Database myDb = null;

        static Db()
        {
            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
        }

        public Db(string databaseName)
        {
            myDb = DatabaseFactory.CreateDatabase(databaseName);
        }

        // fast read and instantiate (i.e. make) a collection of objects
        public IEnumerable<T> Read<T>(string storedProcName, Func<IDataReader, T> make, params DbParameter[] parms)
        {
            using (DbCommand command = myDb.GetStoredProcCommand(storedProcName))
            {
                if (parms != null)
                {
                    foreach (DbParameter param in parms)
                    {
                        if (param.DbType == DbType.Object)
                        {
                            command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value) { SqlDbType = SqlDbType.Structured });
                        }
                        else
                        {
                            myDb.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
                        }

                    }

                }

                using (var reader = myDb.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        yield return make(reader);
                    }
                }
            }
        }


        // return a scalar object
        public object Scalar(string storedProcName, params DbParameter[] parms)
        {
            using (DbCommand command = myDb.GetStoredProcCommand(storedProcName))
            {
                foreach (DbParameter param in parms)
                {
                    myDb.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
                }

                return myDb.ExecuteScalar(command);
            }

        }

        // insert a new record
        public int Insert(string storedProcName, params DbParameter[] parms)
        {
            using (DbCommand command = myDb.GetStoredProcCommand(storedProcName))
            {
                foreach (DbParameter param in parms)
                {
                    myDb.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
                }

                return int.Parse(myDb.ExecuteScalar(command).ToString());

            }
        }

        // update an existing record
        public int Update(string storedProcName, params DbParameter[] parms)
        {
            using (DbCommand command = myDb.GetStoredProcCommand(storedProcName))
            {
                foreach (DbParameter param in parms)
                {
                    myDb.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
                }

                return myDb.ExecuteNonQuery(command);
            }
        }

        // delete a record
        public int Delete(string storedProcName, params DbParameter[] parms)
        {
            return Update(storedProcName, parms);
        }


    }
}
