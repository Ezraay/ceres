using System.Threading.Tasks;
using UnityEngine;

public class SignalRHelper : MonoBehaviour
{
    public SignalRConnector connector;
    public async Task Start()
    {
        connector = new SignalRConnector();
        await connector.InitAsync();
    }
}