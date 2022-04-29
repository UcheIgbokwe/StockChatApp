using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public interface IStockConsumer
    {
        StockModel ConsumeStock { get; }
    }
}