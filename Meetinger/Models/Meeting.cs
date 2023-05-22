using System.ComponentModel.DataAnnotations;
using Meetinger.Areas.Identity.Data;


namespace Meetinger.Models
{
    public class Meeting
    {
        [Key] public Guid Id { get; set; }
        [Required] [MaxLength(100)] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime CreatedDate { get; set; }
        [Required] public DateTime MeetingTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public string CreatorId { get; set; }
        [Required] public ApplicationUser Creator { get; set; }
        [Required] 
        public List<MeetingParticipant> Participants { get; set; }

        public List<ApplicationUser> AvailableUsers { get; set; }
    }
}