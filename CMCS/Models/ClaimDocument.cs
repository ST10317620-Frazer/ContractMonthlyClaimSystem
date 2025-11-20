using CMCS.Models;

namespace ContractLecturerClaimSystem.Models
{
    public class ClaimDocument
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;

        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty; // guid + extension
        public string FilePath => $"/uploads/{StoredFileName}";
    }
}