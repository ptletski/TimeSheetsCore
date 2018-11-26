using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimeSheetsCoreApp.Models;
using TimeSheetsCoreApp.ViewModels;

namespace TimeSheetsCoreApp.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "TimeSheets lets Technical Consultants track their time.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "This application is powered by Fatima Software - Miracles in the Making!";

            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Your privacy is of utmost importance to us.";

            return View();
        }

        public IActionResult SessionFailure()
        {
            return View();
        }

        public IActionResult SessionTimeout()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LoggedInUser loggedInUser = UserSessionManager.InitiateUserSession(
                        model.UserName,
                        model.Password, 
                        HttpContext.Session,
                        this.Response.Cookies);

                    if (loggedInUser != null)
                    {
                        return RedirectToAction("UserTimeSheets", "User", loggedInUser);
                    }
                    else
                    {
                        Debug.Assert(false, "Unexpected result from UserSessionManager.");
                        model.ErrorMsg = "Internal session error. System Administrator contacted.";
                    }
                }
                catch (UserSessionInternalErrorException)
                {
                    model.ErrorMsg = "Internal session error. System Administrator contacted.";
                }
                catch (UserSessionUnregisteredUserException)
                {
                    model.ErrorMsg = "You are not a registered user. Contact your System Administrator.";
                }
                catch (UserSessionUserInExistingSessionException)
                {
                    model.ErrorMsg = "You are already in a session. Contact your System Administrator.";
                }
                catch (UserSessionGeneralFailureException exception)
                {
                    model.ErrorMsg = "Critical error in session. System Administrator contacted.";

                    Debug.Assert(false, exception.Message);
                }
            }
            else
            {
                model.DetermineModelStateErrorMsg();
            }

            return View("Index", model);
        }

        public IActionResult Logout()
        {
            // Wrap up session, then return to Home/Index
            try
            {
                UserSessionManager.EndUserSession(HttpContext.Session, this.Response.Cookies);
            }
            catch (UserSessionCantFindSessionToExitException)
            {
                Debug.Assert(false, "Can't Find Session");
            }
            catch (UserSessionGeneralFailureTryingToExitException exception)
            {
                Debug.Assert(false, "Error Ending Session");
                Debug.Assert(false, exception.Message);
            }

            return View();
        }
    }
}
