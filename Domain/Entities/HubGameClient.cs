namespace Domain.Entities
{
    public class HubGameClient
    {
        public string? LobbyConnectionId { get; set; }
        public string? UserName { get; set; }
        public Guid UserId { get; set; }
        public bool ReadyToPlay { get; set; } = false;
        public Guid GameId { get; set; }
        // Can have whatever you want here
    }
    
}