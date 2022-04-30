using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Client.Data;
using Client.Models;

namespace Client.Infrastructure
{
    public class HttpServices : IHttpServices
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly GroupChatContext _context;
        public HttpServices(IConfiguration config, IHttpClientFactory clientFactory, GroupChatContext context)
        {
            _context = context;
            _clientFactory = clientFactory;
            _config = config;
        }

        public async Task<Message> GetMessage()
        {
            try
            {
                return _context.Message.OrderByDescending(c => c.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Message> GetStock(MessageOut messageOut)
        {
            try
            {
                var client = _clientFactory.CreateClient("brokerPartnerReciever");
                client.DefaultRequestHeaders.Accept.Clear();
                var builder = new UriBuilder(_config["ChatApi:BaseUrl"]);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["stockCode"] = messageOut.StockCode;
                query["groupId"] = messageOut.GroupId.ToString();
                query["addedBy"] = messageOut.AddedBy;
                builder.Query = query.ToString();

                var url = builder.ToString();
                await client.GetAsync(url);
                await Task.Delay(500);
                return Task.Run(async() => await GetMessage()).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}