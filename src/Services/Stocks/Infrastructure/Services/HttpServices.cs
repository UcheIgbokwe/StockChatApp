using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class HttpServices : IHttpServices
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        public HttpServices(IConfiguration config, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _config = config;
        }
        public async Task<StockModel> GetStock(string stockCode)
        {
            var newStockModel = new StockModel();
            try
            {
                var client = _clientFactory.CreateClient("brokerPartner");
                client.DefaultRequestHeaders.Accept.Clear();
                var builder = new UriBuilder(_config["BrokerApi:BaseUrl"]);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["s"] = stockCode;
                query["f"] = _config["BrokerApi:Csv"];
                builder.Query = query.ToString();
                var url = builder.ToString();
                using var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    var data = resp.Split(',');
                    return new StockModel()
                    {
                        Date = !data[0].Contains("N/D") ? Convert.ToDateTime(data[0]) : default,
                        Time = !data[1].Contains("N/D") ? Convert.ToDateTime(data[1]) : default,
                        Open = !data[2].Contains("N/D") ? Convert.ToDouble(data[2]) : default,
                        High = !data[4].Contains("N/D") ? Convert.ToDouble(data[4]) : default,
                        Low = !data[3].Contains("N/D") ? Convert.ToDouble(data[3]) : default,
                        Symbol = data[6],
                        Close = !data[5].Contains("N/D") ? Convert.ToDouble(data[5]) : default,
                        Volume = !data[7].Contains("N/D") ? Convert.ToDouble(data[7]) : default,
                    };
                }
                return newStockModel;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}