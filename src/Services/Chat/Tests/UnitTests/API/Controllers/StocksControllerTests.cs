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
        private readonly Mock<IStocksService> _httpService;
        public StocksControllerTests()
        {
            _httpService = new Mock<IStocksService>();
        }

        [Fact]
        public async Task GetStock_Should_Return_Result()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us", "testGroup", "testUser"));
            var controller = new StocksController(_httpService.Object);

            var result = await controller.GetStock("pl.us", "testGroup", "testUser");

            var resultIsNotNull = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("{ message = Request has been delivered. }", resultIsNotNull?.Value?.ToString());
        }

        [Fact]
        public async Task Getstock_Should_Return_Ok()
        {
            _httpService.Setup(repo => repo.GetStock("aapl.us", "testGroup", "testUser"));
            var controller = new StocksController(_httpService.Object);

            var result = await controller.GetStock("pl.us", "testGroup", "testUser");

            var resultIsNotNull = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, resultIsNotNull.StatusCode);
        }
    }
}