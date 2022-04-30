using System;
using Domain.Entities;
using EventBus.Messages.Events;
using Infrastructure.Data;
using MassTransit;
using PusherServer;

namespace API.EventBusConsumer
{
    public class GetStockConsumer : IConsumer<StockConsumer>
    {
        private readonly ILogger<GetStockConsumer> _logger;
        private readonly ChatDbContext _dbContext;
        public GetStockConsumer(ChatDbContext dbContext, ILogger<GetStockConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<StockConsumer> context)
        {
            var data = context.Message;
            Message stockMessage = new()
            {
                message = $"{data.StockModelDetails.Symbol} quote is ${data.StockModelDetails.Open} per share",
                GroupId = data.GroupId,
                AddedBy = data.AddedBy
            };
            await _dbContext.Message.AddAsync(stockMessage);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("StockEvent consumed successfully. Stock Id: {MessageId}", data.MessageId);
        }
    }
}