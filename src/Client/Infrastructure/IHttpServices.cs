using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Infrastructure
{
    public interface IHttpServices
    {
        Task<Message> GetStock(MessageOut messageOut);
        Task<Message> GetMessage();
    }
}