using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        //Association class between User and Notification. Its key is the combination
        //for user and Notification classes.
        [Column(Order = 1)]
        [Key]
        public string UserId { get; private set; }

        [Column(Order = 2)]
        [Key]
        public int NotificationId { get; private set; }

        public bool IsRead { get; private set; }

        public ApplicationUser User { get; private set; }   //After initializing it can be set therefore private

        public Notification Notification { get; private set; }

        protected UserNotification()
        {

        }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (notification == null)
                throw new ArgumentNullException("notification");
            User = user;
            Notification = notification;
        }

        public void Read()
        {
            IsRead = true;
        }

    }
}