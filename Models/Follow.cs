using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class Follow
    {
        [Column(Order = 1)]
        [Key]
        public string FollowerId { get; set; }
        [Column(Order = 2)]
        [Key]
        public string FolloweeId { get; set; }
        public ApplicationUser Followee { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}