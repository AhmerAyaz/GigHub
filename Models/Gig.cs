using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool isCancelled { get; private set; }

        public ApplicationUser Artist { get; set; } //Navigational properties - They
        //allow us to navigate to a different entity in our domain model

        [Required]
        public string ArtistId { get; set; }    //Foreign Key Property

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }    //For genre we need another class
        //because we dont need to duplicate all these genres in the database.

        [Required]
        public byte GenreId { get; set; }
        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            isCancelled = true;
            var notification = Notification.GigCanceled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);

            }
        }

        public void Modify(string venue, byte genre, DateTime dateTime)
        {
            var notification = Notification.GigUpdated(this, dateTime, venue);

            Venue = venue;
            GenreId = genre;
            DateTime = dateTime;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}