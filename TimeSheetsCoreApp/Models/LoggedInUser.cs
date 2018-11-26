using System;
using System.ComponentModel.DataAnnotations;

namespace TimeSheetsCoreApp.Models
{
    public class LoggedInUser
    {
        public LoggedInUser()
        {
        }

        [Key]
        public int SessionId { get; set; }

        public int UserId { get; set; }
    }
}
