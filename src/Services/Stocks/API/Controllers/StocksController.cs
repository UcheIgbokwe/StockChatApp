using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : Controller
    {
        private readonly IHttpServices _httpServices;

        public StocksController(IHttpServices httpServices)
        {
            _httpServices = httpServices;
        }

        [HttpGet("GetStocks")]
        public async Task<IActionResult> GetStocks([FromQuery] string stockCode)
        {
            var resp = await _httpServices.GetStock(stockCode);
            if(resp.High == 0)
            {
                return NotFound(new { message = "No available stocks"});
            }
            return Ok(resp);
        }
    }
}