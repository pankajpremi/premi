using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Logging
{
    public static class LogUtility
    {
        public static string GetMethodDetails(MethodBase method, params object[] values)
        {
            if (values.Length > 0)
            { 
                ParameterInfo[] parms = method.GetParameters();
                object[] namevalues = new object[2 * parms.Length];
            
                string msg = "Methodname:" + method.Name + ", Input:";
                for (int i = 0, j = 0; i < parms.Length; i++, j += 2)
                {
                    msg += "{" + j + "}={" + (j + 1) + "}, ";
                    namevalues[j] = parms[i].Name;
                    if (i < values.Length) namevalues[j + 1] = values[i];
                }
                msg +=  ")";

               return string.Format(msg, namevalues);
            }

            else
            {
                return method.Name;
            }
        }

     
    }
}
