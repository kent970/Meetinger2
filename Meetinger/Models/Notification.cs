using System.ComponentModel.DataAnnotations;
using Meetinger.Areas.Identity.Data;
namespace Meetinger.Models

{
    public class Notification
    {
        [Key]
        public int NotiId { get; set; } 
        [Required]
        public string FromUserId { get; set; }
        [Required]
        public string ToUserId { get; set; } 
        [Required]
        public string NotiHeader { get; set; }
        [Required]
        public string NotiBody { get; set; } 
        [Required]
        public bool IsRead { get; set; } 
        [Required]
        public string Url { get; set; } 
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Message { get; set; }

        public string CreatedDateSt => this.CreatedDate.ToString("dd-MMM-yyyy HH:mm:ss");
        public string IsReadSt => this.IsRead ? "YES" : "NO";

        public string FromUserName { get; set; }
        public string ToUserName { get; set; } 
    }
}
