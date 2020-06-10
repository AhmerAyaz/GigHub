using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class ReadController : ApiController
    {
        private ApplicationDbContext _context;
        public ReadController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Read()
        {
            var UserId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == UserId && !un.IsRead)
                .ToList();
            notifications.ForEach(n => n.Read());
            _context.SaveChanges();
            return Ok();

        }
    }
}
