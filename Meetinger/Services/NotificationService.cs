using Meetinger.Areas.Identity.Data;
using Meetinger.Models;

namespace Meetinger.Services
{
    public class NotificationService : INotification
    {
        private readonly ApplicationDbContext _context;
        private List<Notification> _notifications;

        public NotificationService(ApplicationDbContext context, List<Notification> notifications)
        {
            _context = context;
            _notifications = notifications;
        }

        public List<Notification> GetNotifications(ApplicationUser user, bool bIsGetOnlyUnread)
        {
            return _context.Notifications.Where(noti => noti.ToUserId == user.Id).  ToList();
        }

        public async Task AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}