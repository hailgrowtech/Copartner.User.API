using CopartnerUser.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.DataAccessLayer.Models
{
    [BsonCollection("sequence")]
    public class Sequence : Document
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
