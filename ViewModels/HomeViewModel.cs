using GigHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.ViewModels
{
    public class HomeViewModel
    {
        public string Heading { get; set; }
        public IEnumerable<Gig> Gigs { get; set; }
        public bool ShowActions { get; set; }
        public string SearchTerm { get; set; }
        public ILookup<int, Attendance> Attendances { get; set; }
        public ILookup<string, Follow> Followings { get; set; }
    }
}