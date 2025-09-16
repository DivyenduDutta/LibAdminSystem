namespace LibAdminSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = "";
        public string Author { get; set; } = "";
        public int Year { get; set; }
        public string Genre { get; set; } = "";
        public int CopiesAvailable { get; set; }

        // Navigation property
        public List<Loan> Loans { get; set; } = new();
    }
}
