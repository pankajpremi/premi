using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class MetadataWrapper<T> where T : class
    {
        public int TotalResults { get; set; }

        public IEnumerable<T> Results { get; set; }

        public DateTime Timestamp { get; set; }

        public MetadataWrapper(IEnumerable<T> results)
        {
            Results = results;
            TotalResults = results.Count();
            Timestamp = DateTime.Now;
        }
    }
}