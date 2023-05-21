using Meetinger.Areas.Identity.Data;
using Meetinger.Models;

namespace Meetinger.Services
{
    public interface IMeeting
    {
        Meeting GetById (int id);
        Task Add(Meeting meeting);
        public IEnumerable<Meeting> GetAll();
        public IEnumerable<Meeting> GetByUser(ApplicationUser user);
        public IEnumerable<Meeting> GetFiltered(string searchQuery);
        public IEnumerable<Meeting> GetLatest(int n);
        Task AddParticipant(Meeting meeting, ApplicationUser id);
    }
}
