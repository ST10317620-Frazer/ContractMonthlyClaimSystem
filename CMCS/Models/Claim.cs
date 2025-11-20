using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerId { get; set; } = "";
        public ApplicationUser Lecturer { get; set; } = null!;

        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount => HoursWorked * HourlyRate;
        public string? Notes { get; set; }
        public string Status { get; set; } = "Pending";
        public string? RejectionReason { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public DateTime? DateProcessed { get; set; }              
        public List<ClaimDocument> Documents { get; set; } = new();
    }
}

    
