namespace CMCS.Models
{
    public class Claim
    {
        public int ClaimID { get; set; }
        public string ClaimName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int HoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; }
         public DateTime? DateProcessed { get; set; }
        public string DocumentPath { get; set; }
    }
}