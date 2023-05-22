using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Meetinger.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Meetinger.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotification _notificationService;
        private List<Notification> _oNotifications;
        private static UserManager<ApplicationUser> _userManager;

        public NotificationsController(INotification notificationService,UserManager<ApplicationUser> userManager, List<Notification> oNotifications)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _oNotifications = oNotifications;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }

        public async Task<JsonResult> GetNotifications(bool bIsGetOnlyUnread = false)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            _oNotifications = new List<Notification>();
            _oNotifications = _notificationService.GetNotifications(user, bIsGetOnlyUnread);
            return Json(_oNotifications);
        }
    }
}
