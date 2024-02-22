using CopartnerUser.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = CopartnerUser.DataAccessLayer.Entities.Document;

namespace CopartnerUser.DataAccessLayer.Models
{
    [BsonCollection("people")]
    public class Person : Document
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
