using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using TimeSheetsCoreApp.Models;
using TimeSheetsCoreApp.ViewModels;
using System.Threading.Tasks;

namespace TimeSheetsCoreApp
{
    public static class UserSessionManager
    {
        static private TimeSheetsDbContext _dbContext;
        static private string _sessionId = "SessionId";

        static UserSessionManager()
        {
            _dbContext = TimeSheetsDbContextFactory.CreateDbContext();
        }

        public static LoggedInUser InitiateUserSession(
            string userName, 
            string password, 
            ISession contextSession,
            IResponseCookies cookies)
        {
            LoggedInUser result = null;
            Exception errorToThrow = null;

            try
            {
                User user = IsUserRegistered(userName, password);

                if (user != null)
                {
                    // Does user have a session running?
                    if (IsUserInExistingSession(user) == true)
                    {   // If so, notify user, deny access
                        errorToThrow = new UserSessionUserInExistingSessionException("User session already exists.");
                    }
                    else
                    {   // If not, establish session, permit access
                        result = CreateSession(user);

                        if (result == null)
                        {
                            errorToThrow = new UserSessionInternalErrorException("Internal Error.");
                        }
                        else
                        {
                            contextSession.SetInt32(_sessionId, result.SessionId);
                            SetCookie(_sessionId, result.SessionId.ToString(), cookies);
                        }
                    }
                }
                else
                {
                    errorToThrow = new UserSessionUnregisteredUserException("User not registered.");
                }
            }
            catch (Exception exception)
            {
                errorToThrow = new UserSessionGeneralFailureException(exception.Message);
            }

            if (errorToThrow != null)
            {
                throw errorToThrow;
            }

            return result;
        }

        public static UserSessionIsValidResult IsUserSessionValid(ISession contextSession, IRequestCookieCollection cookies)
        {
            UserSessionIsValidResult isValidResult = new UserSessionIsValidResult();

            try
            {
                int keyCount = contextSession.Keys.Count();
                isValidResult.IsValid = keyCount != 0;
                isValidResult.SessionId = Int32.Parse(cookies[_sessionId]);

                if (isValidResult.IsValid == false)
                {   // Remove session from application session store.
                    var user = _dbContext.LoggedInUsers.Find(isValidResult.SessionId);
                    isValidResult.UserId = user.UserId;
                }

                Debug.Assert(isValidResult.IsValid, "Keys are missing");
            }
            catch (Exception exception)
            {
                Debug.Assert(false, "Failure With ISession Keys, Store");
                Debug.Assert(false, exception.Message);
            }

            return isValidResult;
        }

        public static void TerminateUserSession(int? sessionId, IResponseCookies cookies)
        {
            try
            {
                if (sessionId != null)
                {
                    var user = _dbContext.LoggedInUsers.Find(sessionId);

                    _dbContext.LoggedInUsers.Remove(user);
                    _dbContext.SaveChanges();
                }

                RemoveCookie(_sessionId, cookies);
            }
            catch (Exception exception)
            {
                throw new UserSessionGeneralFailureTryingToExitException(exception.Message);
            }
        }

        public static void EndUserSession(ISession contextSession, IResponseCookies cookies)
        {
            int? sessionId = contextSession.GetInt32(_sessionId);

            if (sessionId == null)
            {
                throw new UserSessionCantFindSessionToExitException("Can't Find Session.");
            }

            try
            {
                var user = _dbContext.LoggedInUsers.Single(lu => lu.SessionId == sessionId);

                _dbContext.LoggedInUsers.Remove(user);
                _dbContext.SaveChanges();

                RemoveCookie(_sessionId, cookies);
                contextSession.Clear();
            }
            catch(Exception exception)
            {
                throw new UserSessionGeneralFailureTryingToExitException(exception.Message);
            }
        }

        public static UserDetails GetUserDetails(int userId)
        {
            UserDetails details = null;

            try
            {
                User user = _dbContext.Users.Find(userId);

                details = new UserDetails(user.FirstName, user.LastName, user.UserId);
            }
            catch (Exception exception)
            {
                Debug.Assert(false, "Couldn't Find User");
                Debug.Assert(false, exception.Message);
            }

            return details;
        }

        private static User IsUserRegistered(string userName, string password)
        {
            User user = null;

            try
            {
                user = _dbContext.Users.Single(u => u.UserName == userName);

                if (user != null)
                {
                    if (string.Equals(user.Password, password) == false)
                    {
                        user = null;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Assert(false, "User Logon Failure");
                Debug.Assert(false, $"Exception: {exception.Message}");

                throw exception;
            }

            return user;
        }

        private static bool IsUserInExistingSession(User user)
        {
            bool isInSession = false;

            try
            {
                isInSession = _dbContext.LoggedInUsers.Any(lu => lu.UserId == user.UserId);
            }
            catch (Exception exception)
            {
                Debug.Assert(false, "User Session Exists Failure");
                Debug.Assert(false, $"Exception: {exception.Message}");

                throw exception;
            }

            return isInSession;
        }

        private static LoggedInUser CreateSession(User user)
        {
            LoggedInUser loggedInEntry = null;

            try
            {
                LoggedInUser loggedInUser = new LoggedInUser();

                loggedInUser.UserId = user.UserId;

                _dbContext.LoggedInUsers.Add(loggedInUser);
                _dbContext.SaveChanges();

                loggedInEntry = _dbContext.LoggedInUsers.Single(lu => lu.UserId == user.UserId);


            }
            catch (Exception exception)
            {
                Debug.Assert(false, "User Session Creation Failure");
                Debug.Assert(false, $"Exception: {exception.Message}");

                throw exception;
            }

            return loggedInEntry;
        }

        private static void SetCookie(string key, string value, IResponseCookies cookies)
        {
            CookieOptions option = new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(1),
                IsEssential = true
            };

            cookies.Append(key, value, option);
        }

        private static string GetCookie(string cookieKey, IRequestCookieCollection cookies)
        {
            return cookies[cookieKey];
        }

        private static void RemoveCookie(string key, IResponseCookies cookies)
        {
            cookies.Delete(key);
        }
    }
}
