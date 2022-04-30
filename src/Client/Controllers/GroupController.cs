using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly GroupChatContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GroupController(GroupChatContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<UserGroupViewModel> GetAll()
        {
            var groups = _context.UserGroup
                .Where(gp => gp.UserName == _userManager.GetUserName(User))
                .Join(_context.Groups, ug => ug.GroupId, g =>g.ID, (ug,g) =>
                    new UserGroupViewModel(){
                        UserName = ug.UserName,
                        GroupId = g.ID,
                        GroupName = g.GroupName})
                .ToList();

            return groups;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewGroupViewModel group)
        {
            if(!string.IsNullOrEmpty(group.GroupName))
            {
                if( (_context.Groups.Any(gp => gp.GroupName == group.GroupName)) == true ){
                    return new ObjectResult(
                        new { status = "error", message = "group name already exist" }
                    );
                }

                Group newGroup = new Group{ GroupName = group.GroupName };
                // Insert this new group to the database...
                _context.Groups.Add(newGroup);
                _context.SaveChanges();
                //Insert into the user group table, group_id and user_id in the user_groups table...
                foreach( string UserName in group.UserNames)
                {
                    _context.UserGroup.Add(
                        new UserGroup{ UserName = UserName, GroupId = newGroup.ID }
                    );
                    _context.SaveChanges();
                }
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
                    "group_chat", //channel name
                    "new_group", // event name
                new { newGroup } );
                return new ObjectResult(new { status = "success", data = newGroup });
            }
            return BadRequest("Incomplete request");
        }
    }
}