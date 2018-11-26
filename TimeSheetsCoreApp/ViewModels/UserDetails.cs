

namespace TimeSheetsCoreApp.ViewModels
{
    public class UserDetails
    {
        public UserDetails(string firstName, string lastName, int userId)
        {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
    }
}