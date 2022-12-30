namespace Domain.Entities
{
    public class HubGameClient
    {
        public string? UserName { get; set; }
        public bool ReadyToPlay { get; set; } = false;
        // Can have whatever you want here
    }
    
}