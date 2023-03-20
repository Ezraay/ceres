using Newtonsoft.Json;

namespace Ceres.Core.Networking.Messages
{
    public interface INetworkMessage
    {
        [JsonIgnore]
        string MessageName {get;}
    }
}