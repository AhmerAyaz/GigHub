using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        //We need to add attendnce object to DB
        private ApplicationDbContext _context;
        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }
        //ASP.NET Web API does not look for scalar parameter (int) in request body, it expects it to be in the URL
        public IHttpActionResult Attend(AttendanceDto dto)  //We wont get userId for security reasons
        {
            var userId = User.Identity.GetUserId();
            if (_context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId))
                return BadRequest("The attendance already exists");
            //So that the primary key is not duplicated
            Attendance attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unattended(int Id)  //We wont get userId for security reasons
        {
            var userId = User.Identity.GetUserId();
            var attendance = _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == Id);
            if (attendance == null)
                return NotFound();
            _context.Attendances.Remove(attendance);
            _context.SaveChanges();
            return Ok(Id);  //Restful convention is to return Id of what we deleted
            
        }
    }
}