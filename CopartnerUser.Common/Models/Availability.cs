using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.Common.Models
{
    public class Availability
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? AvailabilityTypeId { get; set; }
        public int? Amount { get; set; }
    }
}
