namespace LibAdminSystem.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime JoinDate { get; set; }

        // Navigation property
        public List<Loan> Loans { get; set; } = new();
    }
}
