using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helper;
using Domain.Interface;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stockService;

        public StocksController(IStocksService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("GetStock")]
        public async Task<IActionResult> GetStock([FromQuery] string stockCode, string groupId, string addedBy)
        {
            if(!string.IsNullOrEmpty(stockCode))
            {
                await _stockService.GetStock(stockCode, groupId, addedBy);
                return Ok(new { message = "Request has been delivered."});
            }
            return BadRequest();
        }
    }
}