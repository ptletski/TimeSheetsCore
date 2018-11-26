using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimeSheetsCoreApp.Models;
using TimeSheetsCoreApp.ViewModels;

namespace TimeSheetsCoreApp.Controllers
{
    public class UserController : Controller
    {
        // From HomeController/LogOn
        public ActionResult UserTimeSheets(LoggedInUser user)
        {
            UserSessionIsValidResult validSession = UserSessionManager.IsUserSessionValid(HttpContext.Session, Request.Cookies);

            if (validSession.IsValid == false)
            {   // If this happens at this point, there has been a failure to 
                // initialize the session properly. Termiate.
                UserSessionManager.TerminateUserSession(validSession.SessionId, Response.Cookies);
                return RedirectToAction("SessionFailure", "Home");
            }

            UserDetails details = UserSessionManager.GetUserDetails(user.UserId);
            UserTimeSheetsViewModel timeSheets = new UserTimeSheetsViewModel(details);

            return View(timeSheets);
        }

        // From UserTimeSheets.cshtml
        public IActionResult HoursForWeek(string date, int userId)
        {
            UserSessionIsValidResult validSession = UserSessionManager.IsUserSessionValid(HttpContext.Session, Request.Cookies);

            if (validSession.IsValid == false)
            {   // If this happens at this point, assume session timeout.
                UserSessionManager.TerminateUserSession(validSession.SessionId, Response.Cookies);
                return RedirectToAction("SessionTimeout", "Home");
            }

            UserDetails details = UserSessionManager.GetUserDetails(userId);
            TimeSheetViewModel timeSheet = new TimeSheetViewModel(date, details);

            return View(timeSheet);
        }

        // From HoursForWeek.cshtml
        public IActionResult RefreshUserTimeSheets(int userId)
        {
            UserSessionIsValidResult validSession = UserSessionManager.IsUserSessionValid(HttpContext.Session, Request.Cookies);

            if (validSession.IsValid == false)
            {   // If this happens at this point, assume session timeout.
                UserSessionManager.TerminateUserSession(validSession.SessionId, Response.Cookies);
                return RedirectToAction("SessionTimeout", "Home");
            }

            UserDetails details = UserSessionManager.GetUserDetails(userId);
            UserTimeSheetsViewModel timeSheets = new UserTimeSheetsViewModel(details);

            return View("UserTimeSheets", timeSheets);
        }

        // From HoursForWeek.cshtml
        public IActionResult WorkDayForm(int userId, string date)
        {
            UserSessionIsValidResult validSession = UserSessionManager.IsUserSessionValid(HttpContext.Session, Request.Cookies);

            if (validSession.IsValid == false)
            {   // If this happens at this point, assume session timeout.
                UserSessionManager.TerminateUserSession(validSession.SessionId, Response.Cookies);
                return RedirectToAction("SessionTimeout", "Home");
            }

            UserDetails details = UserSessionManager.GetUserDetails(userId);
            WorkDayFormViewModel model = new WorkDayFormViewModel(details)
            {
                Date = date
            };

            return View("WorkDayForm", model);
        }

        [HttpPost]
        public IActionResult SaveWorkDay(WorkDayFormViewModel model)
        {
            UserSessionIsValidResult validSession = UserSessionManager.IsUserSessionValid(HttpContext.Session, Request.Cookies);

            if (validSession.IsValid == false)
            {   // If this happens at this point, assume session timeout.
                UserSessionManager.TerminateUserSession(validSession.SessionId, Response.Cookies);
                return RedirectToAction("SessionTimeout", "Home");
            }

            try
            {
                if (ModelState.IsValid == false)
                {
                    WorkDayFormViewModel formModel = new WorkDayFormViewModel(model);

                    return View("WorkDayForm", formModel);
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false, "Could Not Save Customer Form");
                Debug.Assert(false, e.Message);
            }

            return RedirectToAction("HoursForWeek", "User", new {date=model.Date, userId=model.UserId});
        }
    }
}
