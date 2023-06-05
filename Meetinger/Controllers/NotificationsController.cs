using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Meetinger.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meetinger.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotification _notificationService;
        private List<Notification> _oNotifications;
        private static UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public NotificationsController(INotification notificationService,UserManager<ApplicationUser> userManager, List<Notification> oNotifications, ApplicationDbContext context)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _oNotifications = oNotifications;
            _context = context;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MarkAllAsRead()
        {
            var notifications = _context.Notifications.ToList();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            _context.SaveChanges();

 
            return Ok();
        }

        [HttpPost]
        public IActionResult MarkAsRead(int notiId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotiId == notiId);
            if (notification == null)
            {
                return NotFound();
            }
            notification.IsRead = true;
            _context.SaveChanges();
            return Ok();
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
