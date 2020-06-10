using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Genre
    {
        public byte Id { get; set; }    //Wont be more than 255
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}