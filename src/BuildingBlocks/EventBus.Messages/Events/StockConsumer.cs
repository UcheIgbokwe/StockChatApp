using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class StockConsumer
    {
        public Guid MessageId { get; set; }
        public StockModel StockModelDetails { get; set; }
        public DateTime CreateDate { get; set; }
    }
}