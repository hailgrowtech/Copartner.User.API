using CopartnerUser.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.DataAccessLayer.Models
{
    [BsonCollection("experts")]
    public class Experts : Document
    {        
        public int ExpertId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Experience { get; set; }
        public int Followers { get; set; }
        public int ExpertTypeId { get; set; }
        public string BioDescription { get; set; }
        public int Rating { get; set; }
        public string Pic {  get; set; }
        public int[] CallAvailabilityIds {  get; set; } 
    }
}
