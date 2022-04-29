using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly GroupChatContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController( GroupChatContext context, UserManager<IdentityUser> userManager){
                _context = context;
                _userManager = userManager;
        }

        [HttpPost]
        public IActionResult ChannelAuth(string channel_name, string socket_id)
        {
            int group_id;
            if(!User.Identity.IsAuthenticated) {
                return new ContentResult { Content = "Access forbidden", ContentType = "application/json" };
            }

            try
            {
                    group_id = Int32.Parse(channel_name.Replace("private-", ""));
            }
            catch (FormatException e)
            {
                return Json( new  { Content = e.Message } );
            }

            var IsInChannel = _context.UserGroup
                                        .Where(gb => gb.GroupId == group_id 
                                            && gb.UserName == _userManager.GetUserName(User))
                                        .Count();

            if( IsInChannel > 0){
                var options = new PusherOptions
                    {
                        Cluster = "ap2",
                        Encrypted = true
                    };
                var pusher = new Pusher(
                    "1403414",
                    "09d9c12236bddfdad2c8",
                    "dd02bbb04c30b4afc34f",
                    options
                );

                var auth = pusher.Authenticate(channel_name, socket_id).ToJson();
                return new ContentResult { Content = auth, ContentType = "application/json" };
            }
            return new ContentResult { Content = "Access forbidden", ContentType = "application/json" };
        }
    }
}