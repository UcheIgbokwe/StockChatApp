using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interface;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : Controller
    {
        private readonly IHttpServices _httpServices;
        private readonly IBus _bus;

        public StocksController(IHttpServices httpServices, IBus bus)
        {
            _bus = bus;
            _httpServices = httpServices;
        }

        [HttpGet("GetStocks")]
        public async Task<IActionResult> GetStocks([FromQuery] string stockCode, string groupId, string addedBy)
        {
            var resp = await _httpServices.GetStock(stockCode);
            if(resp.High == 0)
            {
                return NotFound(new { message = "No available stocks"});
            }
            Uri uri = new($"amqp://localhost:5672/{EventBusConstants.GetStockQueue}");
            var endPoint = await _bus.GetSendEndpoint(uri);
            StockConsumer result = new()
            {
                MessageId = Guid.NewGuid(),
                StockModelDetails = resp,
                AddedBy = addedBy,
                GroupId = Int32.Parse(groupId),
                CreateDate = DateTime.Now
            };
            await endPoint.Send(result);
            return Ok(resp);
        }
    }
}