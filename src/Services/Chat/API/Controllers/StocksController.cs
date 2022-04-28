using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helper;
using Domain.Interface;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stockService;
        private readonly IBus _bus;

        public StocksController(IStocksService stockService, IBus bus)
        {
            _stockService = stockService;
            _bus = bus;
        }

        [HttpGet("GetStocks")]
        public async Task<IActionResult> GetStocks([FromQuery] string stockCode)
        {
            if(!string.IsNullOrEmpty(stockCode))
            {
                var resp = await _stockService.GetStock(stockCode);
                if(resp == null)
                {
                    return NotFound(new { message = "No available stocks"});
                }
                return Ok(resp);
            }
            return BadRequest();
        }
    }
}