using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        public string Action {
            get
            {
                return (Id != 0) ? "Update" : "Create"; //By default Id's are 0. If edit action is not called it remains 0
            }
        }

        public string Heading { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", this.Date, this.Time)); 
        }
    }
}