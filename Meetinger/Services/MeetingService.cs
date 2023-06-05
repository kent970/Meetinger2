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
            return _context.Meetings.Where(m=>m.Id==id).Include(m=>m.Participants).ThenInclude(m=>m.Participant).   FirstOrDefault();
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
            var meetings = _context.Meetings.Include(m => m.Participants).ThenInclude(p => p.Participant)
                .Where(m => m.Participants.Any(mp => mp.Participant == user)).OrderByDescending(m=>m.MeetingTime). ToList();
            return meetings;
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

        public async Task CancelMeeting(Guid meetingId)
        {
            
            if (meetingId != null)
            {
                _context.Meetings.FirstOrDefault(m => m.Id==meetingId).IsCanceled=true;
               // meetingToCancel.IsCanceled = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Meeting not found.");
            }
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

        public async Task Update(Guid id, Meeting meeting)
        {
            var meetingToUpdate = _context.Meetings.FirstOrDefault(m => m.Id == id);

            //AutoMapper
            meetingToUpdate.Participants = meeting.Participants;
            meetingToUpdate.Name = meeting.Name;
            meetingToUpdate.Description = meeting.Description;
            meetingToUpdate.MeetingTime = meeting.MeetingTime;
            meetingToUpdate.EndTime = meeting.EndTime;
            await _context.SaveChangesAsync();
        }
    }
}