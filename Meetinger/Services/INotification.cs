using Meetinger.Areas.Identity.Data;
using Meetinger.Models;

namespace Meetinger.Services
{
    public interface INotification
    {
        public List<Notification> GetNotifications(ApplicationUser user, bool bIsGetOnlyUnread);
        public Task AddNotification(Notification notification);
    }
}
