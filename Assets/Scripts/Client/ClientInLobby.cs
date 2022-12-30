using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;


namespace CardGame
{
    public class ClientInLobby : MonoBehaviour
    {

        public SignalRHelper transport;
        public Text ReadyToPlaylabel;

        public void ToggleValueChanged(bool value)
        {
            if (value) {
                Debug.Log("Ready To Play");
                // ReadyToPlaylabel.text = "Ready To Play";
                transport.connector.connection.SendAsync("UserIsReadyToPlay").GetAwaiter().GetResult();
            } else {
                Debug.Log("Not Ready To Play");
                // ReadyToPlaylabel.text = "Not Ready To Play";
            }
        }

        public void ReadyToPlay(bool value){
            
        }
    }
}
