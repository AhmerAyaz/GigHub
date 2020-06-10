using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpDelete]
        public IHttpActionResult Cancel(int Id)
        {
            var UserId = User.Identity.GetUserId();
            var gig = _context
                .Gigs.Include(g => g.Attendances.Select(a=>a.Attendee))
                .Single(g => g.Id == Id && g.ArtistId == UserId);
            /*var Attendees = _context.Attendances
                .Where(a => a.GigId == gig.Id)
                .Select(a => a.Attendee)
                .ToList();
             Used eager loading   
             */
            if (gig.isCancelled)
                return NotFound();
            gig.Cancel();
            _context.SaveChanges();

            return Ok();
        }
    }
}
