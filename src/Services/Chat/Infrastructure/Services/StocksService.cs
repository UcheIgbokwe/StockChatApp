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
        private readonly IServiceProvider _serviceProvider;
        public StocksService(IServiceProvider serviceProvider, IConfiguration config, IHttpClientFactory clientFactory)
        {
            _serviceProvider = serviceProvider;
            _clientFactory = clientFactory;
            _config = config;
        }

        // public async Task<StockModel> GetStock(string stockCode)
        // {
        //     try
        //     {
        //         var client = _clientFactory.CreateClient("stocksService");
        //         client.DefaultRequestHeaders.Accept.Clear();
        //         var builder = new UriBuilder(_config["StocksAPI:BaseUrl"]);
        //         var query = HttpUtility.ParseQueryString(builder.Query);
        //         query["stockCode"] = stockCode;
        //         builder.Query = query.ToString();

        //         var url = builder.ToString();

        //         using var response = await client.GetAsync(url);
        //         if (response.IsSuccessStatusCode)
        //         {
        //             var resp = JsonConvert.DeserializeObject<StockModel>(response.Content.ReadAsStringAsync().Result);
        //             return resp!;
        //         }
        //         throw new AppException(response.ReasonPhrase!);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new AppException(ex.Message);
        //     }
        // }

        public async Task GetStock(string stockCode)
        {
            try
            {
                var client = _clientFactory.CreateClient("stocksService");
                client.DefaultRequestHeaders.Accept.Clear();
                var builder = new UriBuilder(_config["StocksAPI:BaseUrl"]);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["stockCode"] = stockCode;
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