using System;
using EventBus.Messages.Events;
using MassTransit;

namespace API.EventBusConsumer
{
    public class GetStockConsumer : IConsumer<StockConsumer>
    {
        private readonly ILogger<GetStockConsumer> _logger;
        public GetStockConsumer(ILogger<GetStockConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<StockConsumer> context)
        {
            var data = context.Message;
            _logger.LogInformation("StockEvent consumed successfully. Stock Id: {MessageId}", data.MessageId);
        }
    }
}