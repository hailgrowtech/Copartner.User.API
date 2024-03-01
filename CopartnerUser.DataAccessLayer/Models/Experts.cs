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
        public string Email { get; set; }
        public int MembersInTelegramChannel { get; set; }
        public string FreeTelegramChannelLink { get; set; }
        public string PremiumTelegramChannelLink { get; set; }
        public long Mobile {  get; set; }
        public int Experience { get; set; }
        public string SEBIRegistrationNo { get; set; }
        public int Followers { get; set; }
        public int ExpertType { get; set; }
        public string BioDescription { get; set; }
        public int Rating { get; set; }
        public string Pic {  get; set; }
        public List<CallAvailability> CallAvailabilities { get; set; }
    }
}
