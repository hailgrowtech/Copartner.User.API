using CopartnerUser.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.DataAccessLayer.Models
{
    [BsonCollection("CallAvailabilities")]
    public class CallAvailability : Document
    {
        public int CallAvailabilityId { get; set; }
        public string Time { get; set; }
        public string AMPM { get; set; }
        public bool isActive { get; set; }
    }
}
