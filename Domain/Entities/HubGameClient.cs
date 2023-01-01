namespace Domain.Entities
{
    public class HubGameClient
    {
        public string? UserName { get; set; }
        public bool ReadyToPlay { get; set; } = false;
        public Guid GameId { get; set; }
        // Can have whatever you want here
    }
    
}