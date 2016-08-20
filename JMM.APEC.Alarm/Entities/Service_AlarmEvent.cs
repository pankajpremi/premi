using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm
{
    public class Service_AlarmEvent
    {

        public int AlarmEventId { get; set; }
        public int AtgId { get; set; } //Facility 1 header info from atg table
        public DateTime? AlarmDateTime { get; set; }
        public DateTime? ReceivedDateTime { get; set; }

        public DateTime? RemindDateTime { get; set; }

        public string SourceIP { get; set; }

        public int StateId;
        public DateTime? DispatchDateTime { get; set; }

        public DateTime? ClearDateTime { get; set; }
        public int AppChangeUserId { get; set; }
       
        public int AlarmId;
        public Service_Alarm Alarm;

        public int ResolutionCategoryId;
        public System_Category ResolutionCategory;

        public int ResolutionTypeId;
        public System_Type ResolutionType;        

        public int StatusId;
        public System_Status JMMStatus;

        public int PriorityStatusId;
        public System_Status PriorityStatus;        

        public int SensorId;
        public Asset_Sensor Sensor;
    }
}
