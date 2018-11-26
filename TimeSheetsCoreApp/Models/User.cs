using System;
using System.ComponentModel.DataAnnotations;

namespace TimeSheetsCoreApp.Models
{
    public class User
    {
        public User()
        {
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
