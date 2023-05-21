using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Microsoft.EntityFrameworkCore;

namespace Meetinger.Services
{
    public class MeetingService:IMeeting
    {
        private readonly ApplicationDbContext _context;

        public MeetingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Meeting GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Add(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

        }

        public IEnumerable<Meeting> GetAll()
        {
            return _context.Meetings.Include(meeting => meeting.Creator).Include(meeting => meeting.Participants);

        }

        public IEnumerable<Meeting> GetByUser(ApplicationUser user)
        {
            return _context.Meetings.Where(meeting => meeting.Creator == user);

        }

        public IEnumerable<Meeting> GetFiltered(string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery)
                ? _context.Meetings
                : _context.Meetings.Where(meeting =>
                    meeting.Name.Contains(searchQuery) || meeting.Description.Contains(searchQuery));
        }

        public IEnumerable<Meeting> GetLatest(int n)
        {
            return GetAll().OrderByDescending(meeting => meeting.MeetingTime).Take(n);
        }

        public Task AddParticipant(Meeting meeting, ApplicationUser id)
        {
            throw new NotImplementedException();
        }
    }
}
