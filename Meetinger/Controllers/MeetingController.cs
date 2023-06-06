using System.Net;
using System.Net.Mail;
using Hangfire;
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

            ThisParticipants.Add(new MeetingParticipant
            {
                ParticipantId = user.Id,
                Participant = user,
                Meeting = meeting,
                MeetingId = meeting.Id
            });


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
                IsCanceled = false
            };
            foreach (var participant in ThisParticipants)
            {
                if (participant.Participant != user)
                {
                    await SendNotifications(user, participant,"New meeting","New meeting invitation from:","You have new meeting invitation");
                }
            }


            await _meetingService.Add(createdMeeting);


            return View("CreatedMeeting", createdMeeting);
        }

        private async Task SendNotifications(ApplicationUser user,MeetingParticipant participant,string notiHeader,string notiBody,string message)
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
                FromUserName = string.Concat(user.FirstName, " ", user.LastName),
                ToUserName = string.Concat(participant.Participant.FirstName, " ", participant.Participant.LastName)
            };
            await _notificationService.AddNotification(newMeetingNotification);
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
           // SendEmail();
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var myMeetings = _meetingService.GetByUser(user);
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

        public async Task SetAttendance(bool attendance, Guid meetingId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var meeting = _meetingService.GetById(meetingId);
            var meetingAttendant = _meetingService.GetMeetingAttendant(meeting, user);
            int? mwe = null;
            meetingAttendant.AttendanceStatus = attendance;
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        public async Task CancelMeeting(Guid meetingId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var meetingToCancel = _meetingService.GetById(meetingId);

            foreach (var participant in meetingToCancel.Participants)
            {
                if (participant.Participant != user)
                {
                    await SendNotifications(user, participant, "Meeting canceled", "Meeting canceled by:", "Your meeting have been canceled");
                }
            }

            await _meetingService.CancelMeeting(meetingId);
        }

        public IActionResult UpdateMeeting(Guid id)
        {
            var view = _meetingService.GetById(id);
            view.AvailableUsers = _context.Users.ToList();
            return View(view);
        }

        [HttpPost]
        public async Task UpdateMeetingTask(Guid Id, Meeting meeting, List<string> selectedUsers)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var ThisParticipants = new List<MeetingParticipant>();
            foreach (var userI in selectedUsers)
            {
                {
                    var participant = new MeetingParticipant
                    {
                        ParticipantId = userI,
                        Participant = await _userManager.FindByIdAsync(userI),
                        Meeting = meeting,
                        MeetingId = meeting.Id
                    };
                    ThisParticipants.Add(participant);
                    if (meeting.Participants != null)
                    {
                        meeting.Participants.Clear();
                    }

                    meeting.Participants = ThisParticipants;
                }
            }

            foreach (var participant in ThisParticipants)
            {
                if (participant.Participant != user)
                {
                    await SendNotifications(user, participant, "Meeting update", "Meting updated by:", "Your meeting have been updated");
                }
            }


            await _meetingService.Update(Id, meeting);
        }

        private void CheckMeetingStart()
        {
            RecurringJob.AddOrUpdate(() => ProcessMeetingStart(), Cron.Minutely);
        }

        public async Task ProcessMeetingStart()
        {
            var meetingsToStart = _meetingService.MeetingsToStart();
            foreach (var meeting in meetingsToStart)
            {
                foreach (var user in meeting.Participants)
                {
                    await SendNotifications(meeting.Creator, user, "Meeting started", "Your meeting has started",
                        "Meeting started");
                }
            }
        }
        private void SendEmail()
        {
          
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("meetinger845@gmail.com");
            mail.To.Add("meetinger845@gmail.com");

            mail.Subject = "Hello from ASP.NET MVC!";
            mail.Body = "This is the content of the email.";

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;

            smtpClient.Credentials = new NetworkCredential("meetinger845@gmail.com", "Abcd123@");

            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}