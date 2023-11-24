namespace FinalProject.Models
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string Email { get; set; } // Assuming you use email as the username
        public List<string> AssignedRoles { get; set; } // Roles currently assigned to the user
        public List<string> AvailableRoles { get; set; } // All available roles for assignment

        // Add any other properties that might be useful for your view
    }
}
