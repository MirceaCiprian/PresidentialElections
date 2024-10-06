namespace WebApp1.Models
{
    public class UserRankings
    {

        public string? Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int noVotes { get; set; } = 0;
        public int round { get; set; } = 0;
    }
}
