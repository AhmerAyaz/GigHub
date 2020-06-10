using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs.
                Include(g => g.Artist).
                Include(g => g.Genre).
                Where(g => g.DateTime > DateTime.Now && !g.isCancelled);

            if(!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(g =>
                g.Artist.Name.Contains(query) ||
                g.Genre.Name.Contains(query) ||
                g.Venue.Contains(query));
            }

            var UserId = User.Identity.GetUserId();
            var attendance = _context.Attendances
                .Where(a => a.AttendeeId == UserId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var follow = _context.Follow
                .Where(a => a.FollowerId == UserId)
                .ToList()
                .ToLookup(a => a.FolloweeId);


            var viewModel = new HomeViewModel()
            {
                Gigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendance,
                Followings = follow
            };
            return View("Gig", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}