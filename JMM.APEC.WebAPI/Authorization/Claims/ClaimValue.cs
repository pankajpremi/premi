using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Authorization
{
    public abstract class ClaimValue
    {
        public abstract string ValueType();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}