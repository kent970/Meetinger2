using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Microsoft.EntityFrameworkCore;

namespace Meetinger.Services
{
    public class MeetingService : IMeeting
    {
        private readonly ApplicationDbContext _context;

        public MeetingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Meeting? GetById(Guid id)
        {
            return _context.Meetings.FirstOrDefault(meeting => meeting.Id == id);
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
            // IEnumerable<Meeting> ParticipantMeetings = _context.Meetings.Where(meeting => meeting.Participants.)
            //
            //
            //     IEnumerable<Meeting> userMeets
            return _context.Meetings.Where(meeting => meeting.Creator == user);
        }

        public IEnumerable<Meeting> GetByParticipant(ApplicationUser participant)
        {
            return _context.Participants.Include(mp => mp.Participant).Where(mp => mp.ParticipantId == participant.Id)
                .Include(mp => mp.Meeting).ThenInclude(mp => mp.Creator).Select(mp => mp.Meeting).ToList();
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

        public Task AddParticipant(Meeting meeting, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public MeetingParticipant GetMeetingAttendant(Meeting meeting, ApplicationUser user)
        {
            foreach (var mp in _context.Participants)
            {
                if (mp.Meeting == meeting && mp.Participant == user)
                {
                    MeetingParticipant meetingAttendant = mp;
                    return meetingAttendant;
                }
            }

            return null;
        }
    }
}