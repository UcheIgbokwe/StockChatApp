using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.Events;

namespace Domain.Interface
{
    public interface IStocksService
    {
        Task<StockModel> GetStock(string stockCode);
    }
}