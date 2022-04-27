using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Domain.Entities;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.UnitTests.API.Controllers
{
    public class StocksControllerTests
    {
        private readonly Mock<IHttpServices> _httpService;
        public StocksControllerTests()
        {
            _httpService = new Mock<IHttpServices>();
        }

        [Fact]
        public async Task GetAllPropertyData_Should_Return_Null()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us"))
                            .ReturnsAsync(new StockModel());
            var controller = new StocksController(_httpService.Object);

            var result = await controller.GetStocks("aapl.us");

            var resultIsNull = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("{ message = No available stocks }", resultIsNull?.Value?.ToString());
        }

        [Fact]
        public async Task GetAllPropertyData_Should_Return_NotFound()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us"))
                            .ReturnsAsync(new StockModel());
            var controller = new StocksController(_httpService.Object);

            var result = await controller.GetStocks("aapl.us");

            var resultIsNotNull = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, resultIsNotNull.StatusCode);
        }
    }
}