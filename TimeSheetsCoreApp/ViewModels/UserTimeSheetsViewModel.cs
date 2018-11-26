using System;
using TimeSheetsCoreApp.Models;

namespace TimeSheetsCoreApp.ViewModels
{
    public class UserTimeSheetsViewModel
    {
        public UserTimeSheetsViewModel(UserDetails user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserId = user.UserId;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UserId { get; set; }

        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string[] TimeSheetDatesAvailable()
        {
            string[] results = new string[2];

            results[0] = "10/08/2018";  // strict format
            results[1] = "10/15/2018";

            return results;
        }
    }
}
