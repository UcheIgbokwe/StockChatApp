using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using EventBus.Messages.Events;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MassTransit;

namespace Tests.UnitTests.API.Controllers
{
    public class StocksControllerTests
    {
        private readonly Mock<IHttpServices> _httpService;
        private readonly Mock<IBus> _bus;
        public StocksControllerTests()
        {
            _httpService = new Mock<IHttpServices>();
            _bus = new Mock<IBus>();
        }

        [Fact]
        public async Task GetAllPropertyData_Should_Return_Null()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us"))
                            .ReturnsAsync(new StockModel());
            var controller = new StocksController(_httpService.Object, _bus.Object);

            var result = await controller.GetStocks("pl.us", "testGroup", "testUser");

            var resultIsNull = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("{ message = No available stocks }", resultIsNull?.Value?.ToString());
        }

        [Fact]
        public async Task GetAllPropertyData_Should_Return_NotFound()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us"))
                            .ReturnsAsync(new StockModel());
            var controller = new StocksController(_httpService.Object, _bus.Object);

            var result = await controller.GetStocks("pl.us", "testGroup", "testUser");

            var resultIsNotNull = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, resultIsNotNull.StatusCode);
        }
    }
}