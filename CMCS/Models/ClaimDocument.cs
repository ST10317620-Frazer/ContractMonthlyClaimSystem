namespace CMCS.Models
{
    public class ClaimDocument
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;

        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;

        public string FilePath => $"/uploads/{StoredFileName}";
    }
}