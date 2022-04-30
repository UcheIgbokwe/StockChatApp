using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data;
using Client.Infrastructure;
using Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly GroupChatContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpServices _httpServices;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger, IHttpServices httpServices, GroupChatContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _httpServices = httpServices;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{group_id}")]
        public IEnumerable<Message> GetById(int group_id)
        {
            return _context.Message.Where(gb => gb.GroupId == group_id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageViewModel message)
        {
            if (!string.IsNullOrEmpty(message.message))
            {
                if(message.message.ToLower().Contains("/stock"))
                {
                    string newStringcode = message.message.Replace("/stock=", "");
                    MessageOut messageOut = new MessageOut{
                        StockCode = newStringcode,
                        AddedBy = _userManager.GetUserName(User),
                        GroupId = message.GroupId,
                    };
                    var messageResult = await _httpServices.GetStock(messageOut);
                    Message new_message = new Message { AddedBy = _userManager.GetUserName(User), message = messageResult.message, GroupId = message.GroupId };

                    return new ObjectResult(new { status = "success", data = new_message });
                }else{
                    Message new_message = new Message { AddedBy = _userManager.GetUserName(User), message = message.message, GroupId = message.GroupId };

                    _context.Message.Add(new_message);
                    _context.SaveChanges();

                    var options = new PusherOptions
                    {
                        Cluster = "ap2",
                        Encrypted = true
                    };
                    var pusher = new Pusher(
                        "1403414",
                        "b1123b7993f96bea3321",
                        "a8073a8a34baee9d319f",
                        options
                    );
                    var result = await pusher.TriggerAsync(
                        "private-" + message.GroupId,
                        "new_message",
                    new { new_message },
                    new TriggerOptions() { SocketId = message.SocketId });

                    return new ObjectResult(new { status = "success", data = new_message });
                }
            }
            return BadRequest("Incomplete request");
        }
    }
}