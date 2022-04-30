using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EventBus.Messages.Events;
using Domain.Interface;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public class StocksService : IStocksService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        public StocksService(IConfiguration config, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        public async Task GetStock(string stockCode, string groupId, string addedBy)
        {
            try
            {
                var client = _clientFactory.CreateClient("stocksService");
                client.DefaultRequestHeaders.Accept.Clear();
                var builder = new UriBuilder(_config["StocksAPI:BaseUrl"]);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["stockCode"] = stockCode;
                query["groupId"] = groupId;
                query["addedBy"] = addedBy;
                builder.Query = query.ToString();

                var url = builder.ToString();

                await client.GetAsync(url);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}