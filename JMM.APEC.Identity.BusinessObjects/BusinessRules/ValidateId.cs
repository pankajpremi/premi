﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects.BusinessRules
{
    public class ValidateId : BusinessRule
    {
        public ValidateId(string propertyName)
            : base(propertyName)
        {
            Error = propertyName + " is an invalid identifier";
        }

        public ValidateId(string propertyName, string errorMessage)
            : base(propertyName)
        {
            Error = errorMessage;
        }

        public override bool Validate(BusinessObject businessObject)
        {
            try
            {
                int id = int.Parse(GetPropertyValue(businessObject).ToString());
                return id >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}