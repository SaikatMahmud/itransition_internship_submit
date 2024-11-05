using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Clients;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class IntegrationController : Controller
    {
        private UserService userService;
        private readonly SalesforceClient salesforceClient;
        private readonly JiraClient jiraClient;

        public IntegrationController(IBusinessService serviceAccess, SalesforceClient _salesforceClient, JiraClient _jiraClient)
        {
            userService = serviceAccess.UserService;
            salesforceClient = _salesforceClient;
            jiraClient = _jiraClient;
        }

        #region Salesforce       

        [HttpGet, UserAccess]
        [Route("sf/add-my-data")]
        public IActionResult SaleforceAccountForm()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userData = userService.Get(u => u.UserId == userId);
            return View(userData);
        }

        [HttpPost, UserAccess]
        [Route("sf/add-my-data")]
        public async Task<IActionResult> CreateSaleforceAccount(SalesforceAccount sfAccount)
        {
            var response = await salesforceClient.CreateAccount(sfAccount);
            if(string.IsNullOrEmpty(response))
            {
                TempData["error"] = "Failed to add data. Try again later!";
                return Redirect("/sf/add-my-data");
            }
            TempData["success"] = "Data added successfully";
            return Redirect("/");
        }


        [HttpGet, UserAccess(SD.Role.Admin)]
        [Route("admin/sf/add-user-data/{userId}")]
        public IActionResult SaleforceAccountForm(int userId)
        {
            if(userId == 0)
            {
                return NotFound();
            }
            var userData = userService.Get(u => u.UserId == userId);
            if(userData == null)
            {
                return NotFound();
            }
            return View(userData);
        }

        [HttpPost, UserAccess(SD.Role.Admin)]
        [Route("admin/sf/add-user-data/{userId}")]
        public async Task<IActionResult> AdminCreateSaleforceAccount(SalesforceAccount sfAccount)
        {
            var response = await salesforceClient.CreateAccount(sfAccount);
            if (string.IsNullOrEmpty(response))
            {
                TempData["error"] = "Failed to add data. Try again later!";
            }
            else TempData["success"] = "Account created successfully";
            return Redirect("/admin/manage-user");
        }

        #endregion

        #region Jira

        [HttpGet, UserAccess]
        [Route("create-support-ticket")]
        public async Task<IActionResult> JiraTicketForm()
        {
            var referenceUrl = Request.Headers.Referer.ToString();
            ViewBag.referenceUrl = referenceUrl;
            return View();
        }

        [HttpPost, UserAccess]
        [Route("create-support-ticket")]
        public async Task<IActionResult> JiraTicketForm(string summary, string priority, string referenceUrl)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = userService.Get(u => u.UserId == userId);
            var result = await jiraClient.CreateTicket(user.Email, summary, priority, referenceUrl);
            if (result) { 
                TempData["success"] = "Ticket created successfully";
                return Redirect("/my-tickets");
            }
            TempData["error"] = "Failed to create ticket. Try again later!";
            return Redirect("/create-support-ticket");
        }

        [HttpGet, UserAccess]
        [Route("my-tickets")]
        public IActionResult MyTicketList()
        {
            return View();
        }

        [HttpGet, UserAccess]
        [Route("get-my-tickets")]
        public async Task<JsonResult> GetMyTicketList(int draw, int start, int length)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await jiraClient.GetUserTicketList(userEmail, start, length);
            if(string.IsNullOrEmpty(result))
            {
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new object[] { } });
            }

            var issueList = JsonConvert.DeserializeObject<JiraUserIssueList>(result)!;
            var response = new
            {
                draw = draw,
                recordsTotal = issueList.total,
                recordsFiltered = issueList.total,
                data = issueList.issues
            };
            return Json(response);
        }

        #endregion
    }
}
