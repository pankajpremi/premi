using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetPhoneAssignmentDto
    {
        public int PhoneAssignmentID { get; set; }
        public int PhoneId { get; set; }
        public int ObjectId { get; set; }
        public int EntityId { get; set; }
        public string Number { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
    }
}