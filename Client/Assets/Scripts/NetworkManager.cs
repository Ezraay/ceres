using System;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Ceres.Client
{
    // [CreateAssetMenu(menuName = "Create NetworkManager", fileName = "NetworkManager", order = 0)]
    public static class NetworkManager
    {
        public static event Action OnConnected; // TODO: Call this when connected
        public static event Action OnJoinGame;
        private static SignalRConnector transport;


        static NetworkManager(){
            transport = new SignalRConnector();
            Debug.Log("OnEnable called");                
        }
        
        public static async Task Connect()
        {
            // TODO: Connect
            await transport.InitAsync();

            transport.connection.On<string>("GoToGame", (gameId) =>
            {
                MainThreadManager.Execute(OnGoToGame);
            });

            if (transport.connection.State == HubConnectionState.Connected){
               OnConnected.Invoke();
            }
        }

        public static async Task JoinQueue()
        {
            // TODO: Join queue
            Debug.Log("JoinQueue called");
            string userName = "Aaron";
            await transport.connection.SendAsync("UserIsReadyToPlay",userName,true);
            Debug.Log("UserIsReadyToPlay passed");
        }
        private static void OnGoToGame(){
            Debug.Log("OnGotToGame called");
            OnJoinGame.Invoke();
            Debug.Log("OnJoinGame.Invoke() passed");
        }
    }
}