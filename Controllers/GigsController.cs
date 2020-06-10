using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpPost]
        public ActionResult Search(HomeViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        public ActionResult Details(int Id)
        {
            var UserId = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == Id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == UserId);

                viewModel.IsFollowing = _context.Follow
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == UserId);
            }
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var UserId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == UserId && g.DateTime > DateTime.Now && !g.isCancelled)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }

        [Authorize]
        public ActionResult Following()
        {
            var UserId = User.Identity.GetUserId();
            var Artist = _context.Follow.Where(f => f.FollowerId == UserId).Select(f => f.Followee).ToList();
            FollowingArtist MyArtists = new FollowingArtist();
            MyArtists.FollowedArtist = Artist;
            return View(MyArtists);
            
        }

        [Authorize]
        public ActionResult Attending()
        {
            var UserId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == UserId)
                .Select(a => a.Gig)
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .ToList();

            var attendance = _context.Attendances
                .Where(a => a.AttendeeId == UserId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var viewModel = new HomeViewModel()
            {
                ShowActions = User.Identity.IsAuthenticated,
                Gigs = gigs,
                Heading = "Gigs I'm Attending",
                Attendances = attendance
            };
            return View("Gig", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int Id)
        {
            var userId = User.Identity.GetUserId();
            var Gig = _context.Gigs.Single(g => g.Id == Id && g.ArtistId == userId);
            var ViewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Date = Gig.DateTime.ToString("d MMM yyyy"),
                Time = Gig.DateTime.ToString("HH:mm"),
                Genre = Gig.GenreId,
                Venue = Gig.Venue,
                Id = Gig.Id,
                Heading = "Edit a Gig"
            };
            return View("GigForm", ViewModel);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]  //We want it to be called only via HTTPPost 
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var UserId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == UserId);

            gig.Modify(viewModel.Venue, viewModel.Genre, viewModel.GetDateTime());
            
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),   //To populate list
                Heading = "Add a Gig"
            };
            return View("GigForm", ViewModel);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]  //We want it to be called only via HTTPPost 
        public ActionResult Create(GigFormViewModel viewModel)
        {
            //u => u.Id == User.Identity.GetUserId() cannot be converted to SQL statements by EF as purely C#.
            //var artist = _context.Users.Single(u => u.Id == artistID);
            //var genre = _context.Genres.Single(g => g.Id == viewModel.Genre);
            //These 2 statements issue 2 Queries to the DB. 2 Unnecessarey round trips.
            if(!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();    //Same as in Create method
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),//Id of currently logged in user
                DateTime = viewModel.GetDateTime(),//Because of reflection when !IsValid
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            //Seperation of concerns, DateTime parse in GigFormViewModel.
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }


        
    }
}