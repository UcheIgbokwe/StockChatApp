using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.Events;
using MassTransit;

namespace API.EventBusConsumer
{
    public class GetStockConsumer : IConsumer<StockModel>
    {
        private readonly ILogger<GetStockConsumer> _logger;
        public GetStockConsumer(ILogger<GetStockConsumer> logger)
        {
            _logger = logger;

        }
        public async Task Consume(ConsumeContext<StockModel> context)
        {
            var data = context.Message;
            _logger.LogInformation("StockEvent consumed successfully. Stock Symbol: {Symbol}", data.Symbol);
        }
    }
}