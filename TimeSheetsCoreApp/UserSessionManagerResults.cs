using System;
using TimeSheetsCoreApp.Models;

namespace TimeSheetsCoreApp
{
    public class UserSessionIsValidResult
    {
        public UserSessionIsValidResult()
        {
            IsValid = false;
            SessionId = null;
            UserId = null;
        }

        public bool IsValid { get; set ; }
        public int? UserId { get; set; }
        public int? SessionId { get; set; }
    }

    public class UserSessionInternalErrorException : Exception
    {
        public UserSessionInternalErrorException(string message) : base(message)
        {
        }
    }

    public class UserSessionUnregisteredUserException : Exception
    {
        public UserSessionUnregisteredUserException(string message) : base(message)
        {
        }
    }

    public class UserSessionUserInExistingSessionException : Exception
    {
        public UserSessionUserInExistingSessionException(string message) : base(message)
        {
        }
    }

    public class UserSessionCantFindSessionToExitException : Exception
    {
        public UserSessionCantFindSessionToExitException(string message) : base(message)
        {
        }
    }

    public class UserSessionGeneralFailureTryingToExitException : Exception
    {
        public UserSessionGeneralFailureTryingToExitException(string message) : base(message)
        {
        }
    }

    public class UserSessionGeneralFailureException : Exception
    {
        public UserSessionGeneralFailureException(string message) : base(message)
        {
        }
    }
}