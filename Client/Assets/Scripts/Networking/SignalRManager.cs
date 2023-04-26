using System;
using System.Threading.Tasks;
using Ceres.Core.Networking.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;

namespace Ceres.Client.Networking
{
    public class SignalRManager : MonoBehaviour
    {
        private readonly string connectionString = "http://localhost:5146";
        private readonly string gameHubPath = "GameHub";
        private readonly string lobbyHubPath = "LobbyHub";
        private HubConnection lobbyHub;
        private HubConnection gameHub;

        public async void Connect()
        {
            lobbyHub = CreateHubConnection(connectionString, lobbyHubPath);
            gameHub = CreateHubConnection(connectionString, gameHubPath);

            try
            {
                await lobbyHub.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Log("Error connecting to Lobby: " + ex.Message);
                Logger.Log("StackTrace: " + ex.StackTrace);
            }

            Logger.Log("Connected to Lobby");

            OnLobbyConnected?.Invoke();
        }

        public event Action OnLobbyConnected;

        public async Task ConnectToGameHubAsync()
        {
            await gameHub.StartAsync();
        }
        public async Task DisconnectFromGameHubAsync()
        {
            await gameHub.StopAsync();
        }
        
        public void OnLobbyMessage<T>(Action<T> callback) where T : INetworkMessage, new()
        {
            var temporaryMessage = new T();
            lobbyHub.On(temporaryMessage.MessageName, callback);
        }

        public void OnGameMesage<T>(Action<T> callback) where T : INetworkMessage, new()
        {
            T temporaryMessage = new T();
            gameHub.On(temporaryMessage.MessageName, callback);
        }

        // public async Task<T> SendAsync<T>(HubConnection connection, INetworkMessage message)
        // {
        //     T result = await connection.InvokeAsync<T>(message.MessageName, message);
        //     return result;
        // }

        public async void SendToLobbyAsync(INetworkMessage message)
        {
            await lobbyHub.SendAsync(message.MessageName, message);
        }
        public async void SendToGameAsync(INetworkMessage message)
        {
            await gameHub.SendAsync(message.MessageName, message);
        }

        private HubConnection CreateHubConnection(string hostname, string hubName)
        {
            string url = $"{hostname}/{hubName}";
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                    // options.PayloadSerializerSettings.
                })
                .Build();

            return connection;
        }

    }
}