using Meetinger.Areas.Identity.Data;
using Meetinger.Models;

namespace Meetinger.Services
{
    public interface IMeeting
    {
        Meeting? GetById (Guid id);
        Task Add(Meeting meeting);
        public IEnumerable<Meeting> GetAll();
        public IEnumerable<Meeting> GetByUser(ApplicationUser user);
        public IEnumerable<Meeting> GetFiltered(string searchQuery);
        public IEnumerable<Meeting> GetLatest(int n);
        Task CancelMeeting(Guid meetingId);
        Task AddParticipant(Meeting meeting, ApplicationUser user);
        MeetingParticipant GetMeetingAttendant(Meeting meeting,ApplicationUser user);
        Task Update(Guid id, Meeting meeting);
       

    }
}
