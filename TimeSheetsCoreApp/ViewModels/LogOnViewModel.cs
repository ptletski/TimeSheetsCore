using System;
using System.ComponentModel.DataAnnotations;

namespace TimeSheetsCoreApp.ViewModels
{
    public class LogOnViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string ErrorMsg { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public void DetermineModelStateErrorMsg()
        {
            bool isMissingUserName = false;
            bool isMissingPassword = false;

            if (string.IsNullOrEmpty(UserName))
                isMissingUserName = true;
            if (string.IsNullOrEmpty(Password))
                isMissingPassword = true;

            if (isMissingUserName && isMissingPassword)
                ErrorMsg = "Please enter user name and password.";
            else if (isMissingUserName)
                ErrorMsg = "Please enter your user name.";
            else if (isMissingPassword)
                ErrorMsg = "Please enter your password.";
            else
                ErrorMsg = "Internal error.";
        }
    }
}
