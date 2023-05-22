using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Meetinger.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Meetinger.Controllers
{
    public class MeetingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMeeting _meetingService;
        private readonly INotification _notificationService;

        public MeetingController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IMeeting meetingService, INotification notificationService)
        {
            _userManager = userManager;
            _context = context;
            _meetingService = meetingService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(Meeting meeting, List<string> selectedUsers)
        {
            meeting.CreatedDate = DateTime.Now;
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            List<ApplicationUser> users = _context.Users.ToList();
            var ThisParticipants = new List<MeetingParticipant>();
            foreach (var userI in selectedUsers)
            {
                var participant = new MeetingParticipant
                {
                    ParticipantId = userI,
                    Participant = await _userManager.FindByIdAsync(userI),
                    Meeting = meeting,
                    MeetingId = meeting.Id
                };
                ThisParticipants.Add(participant);
            }


            var createdMeeting = new Meeting
            {
                Id = Guid.NewGuid(),
                MeetingTime = meeting.MeetingTime,
                EndTime = meeting.EndTime,
                CreatedDate = DateTime.Now,
                Creator = user,
                Participants = ThisParticipants,
                Name = meeting.Name,
                Description = meeting.Description,
            };
            foreach (var participant in ThisParticipants)
            {
                var newMeetingNotification = new Notification
                {
                   // NotiId = Guid.NewGuid(),
                    FromUserId = user.Id,
                    ToUserId = participant.ParticipantId,
                    NotiHeader = "New meeting",
                    NotiBody = "New meeting invitation from",
                    IsRead = false,
                    Url = "/Notifications/AllNotifications",
                    CreatedDate = DateTime.Now,
                    Message = "New invitation for meeting",
                    FromUserName = user.FirstName,
                    ToUserName = participant.Participant.FirstName

                };
                await _notificationService.AddNotification(newMeetingNotification);
            }


            await _meetingService.Add(createdMeeting);


            return View("CreatedMeeting", createdMeeting);
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipant(string userId, Guid meetingId)
        {
            var meeting = _meetingService.GetById(meetingId);
            if (meeting == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);


            return Ok();
        }


        public IActionResult AllMeetings()
        {
            var allMeetings = _meetingService.GetAll();
            return View(allMeetings);
        }

        public async Task<IActionResult> MyMeetings()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var myMeetings = _meetingService.GetByParticipant(user);
            return View(myMeetings);
        }

        public IActionResult NewMeeting()
        {
            List<ApplicationUser> users = _context.Users.ToList();
            Meeting viewModel = new Meeting()
            {
                AvailableUsers = users,
                Participants = new List<MeetingParticipant>()
            };

            return View(viewModel);
        }

        public async Task SetAttendance(bool attendance,Guid meetingId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var meeting = _meetingService.GetById(meetingId);
            var meetingAttendant = _meetingService.GetMeetingAttendant(meeting,user);
            int? mwe = null;
            meetingAttendant.AttendanceStatus = attendance;
            await _context.SaveChangesAsync();
        }
    }
}