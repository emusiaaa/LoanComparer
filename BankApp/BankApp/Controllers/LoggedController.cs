using BankApp.Models;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Authorize]
    public class LoggedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> HistoryOfInquiries(int pageNumber = 1, int pageSize = 15)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = _loggedInquiryRepository.GetAll(user.Id);
            var result = new PagedResult<InquiryModel>
            {
                Data = model.Skip(excludeRecords).Take(pageSize).ToList(),
                TotalItems = model.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }
    }
}
