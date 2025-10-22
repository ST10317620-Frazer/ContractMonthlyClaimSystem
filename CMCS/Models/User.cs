namespace CMCS.Models
{
    public class User
        {
        public int UserID { get; set;}
        public required string Username { get; set; }
        public required string PasswordHash { get; set; } //password funtionality
        public required string Role { get; set; }//establish roles to implement role based access
        public DateTime? DateProcessed { get; set; }// time that the admin will either approve or reject claim
    }
}