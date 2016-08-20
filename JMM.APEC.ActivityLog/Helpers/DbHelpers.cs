using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog.Helpers
{
    public static class DbHelpers
    {
        public static List<int> MakeParamIntList(string InputValue)
        {
            if (InputValue == null)
            {
                return null;
            }

            List<string> strInputList = new List<string>(InputValue.Split(new char[] { ',' }));
            List<int> OutputList = new List<int>();

            if (strInputList.Count > 0)
            {
                foreach (string si in strInputList)
                {
                    if (!(si == string.Empty))
                    {
                        int ItemId = 0;
                        int.TryParse(si, out ItemId);

                        OutputList.Add(ItemId);
                    }
                }

                return OutputList;
            }

            return null;
        }

        public static List<SqlDataRecord> MakeParamRecordList(List<int> InputValue, string IdName)
        {
            if (InputValue == null)
            {
                return null;
            }

            List<SqlDataRecord> OutputList = new List<SqlDataRecord>();

            if (InputValue.Count > 0)
            {
                foreach (var si in InputValue)
                {
                    SqlDataRecord fsdr = new SqlDataRecord(new SqlMetaData[] { new SqlMetaData(IdName, SqlDbType.Int) });
                    fsdr.SetInt32(0, si);
                    OutputList.Add(fsdr);
                }

                return OutputList;
            }

            return null;
        }
    }
}
