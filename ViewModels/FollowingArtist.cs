using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.ViewModels
{
    public class FollowingArtist
    {
        public IEnumerable<ApplicationUser> FollowedArtist { get; set; }
    }
}