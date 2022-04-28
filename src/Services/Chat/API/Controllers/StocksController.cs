using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helper;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stockService;

        public StocksController(IStocksService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("GetStocks")]
        public async Task<IActionResult> GetStocks([FromQuery] string stockCode)
        {
            var resp = await _stockService.GetStock(stockCode);
            if(resp == null)
            {
                return NotFound(new { message = "No available stocks"});
            }
            return Ok(resp);
        }
    }
}