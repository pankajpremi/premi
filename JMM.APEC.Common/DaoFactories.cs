using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Common.Interfaces;

namespace JMM.APEC.Common
{
    // Factory of factories. This class is a factory class that creates
    // data-base specific factories which in turn create data acces objects.
    // ** GoF Design Patterns: Factory.

    public class DaoFactories
    {
        // gets a provider specific (i.e. database specific) factory 

        // ** GoF Design Pattern: Factory

        public static IDaoFactory GetFactory(string dataProvider)
        {
            // return the requested DaoFactory

            switch (dataProvider.ToLower())
            {
                case "enterpriselibrary": return new EnterpriseLibrary.DaoFactory();

                default: return new EnterpriseLibrary.DaoFactory();
            }
        }

    }
}
