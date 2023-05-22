using System.ComponentModel.DataAnnotations.Schema;
using Meetinger.Areas.Identity.Data;


namespace Meetinger.Models
{
    public class MeetingParticipant
    {
        public string ParticipantId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser Participant { get; set; }

        public Guid MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public Meeting Meeting { get; set; }
        public bool AttendanceStatus { get; set; }

    }
}
