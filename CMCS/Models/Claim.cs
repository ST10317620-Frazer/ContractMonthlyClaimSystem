using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class Claim
    {
       public int Id { get; set; }

        public string LecturerId { get; set; } = string.Empty;
        public ApplicationUser Lecturer { get; set; } = null!;

        [Required]
        public decimal HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        [NotMapped]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public string? RejectionReason { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public DateTime? ReviewedDate { get; set; }

        public List<ClaimDocument> Documents { get; set; } = new();
    }
}