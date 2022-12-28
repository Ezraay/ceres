using System.Threading.Tasks;
using UnityEngine;

public class SignalRHelper : MonoBehaviour
{
    private SignalRConnector connector;
    public async Task Start()
    {
        connector = new SignalRConnector();
        await connector.InitAsync();
    }
}