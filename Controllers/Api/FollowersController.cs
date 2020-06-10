using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowersController : ApiController
    {
        private ApplicationDbContext _context;

        public FollowersController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowDto Id)
        {
            var UserId = User.Identity.GetUserId();
            if (_context.Follow.Any(f => f.FollowerId == UserId && f.FolloweeId == Id.Followee))
                return BadRequest("Following Already Exists");
            Follow follow = new Follow()
            {
                FolloweeId = Id.Followee,
                FollowerId = UserId
            };
            _context.Follow.Add(follow);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string Id)  //We wont get userId for security reasons
        {
            var userId = User.Identity.GetUserId();
            var follow = _context.Follow
                .SingleOrDefault(a => a.FollowerId == userId && a.FolloweeId == Id);
            if (follow == null)
                return NotFound();
            _context.Follow.Remove(follow);
            _context.SaveChanges();
            return Ok(Id);  //Restful convention is to return Id of what we deleted

        }
    }
}
