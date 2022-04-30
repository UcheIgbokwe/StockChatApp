using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class MessageOut
    {
        public string AddedBy { get; set;  }
        public int GroupId { get; set;  }
        public string StockCode { get; set; }
    }
}