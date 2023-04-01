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
        public HubConnection LobbyHub { get; private set; }
        public HubConnection GameHub { get; private set; }

        public async void Connect()
        {
            LobbyHub = CreateHubConnection(connectionString, lobbyHubPath);
            GameHub = CreateHubConnection(connectionString, gameHubPath);

            try
            {
                await LobbyHub.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Log("Error connecting to the SignalR server: " + ex.Message);
                Logger.Log("StackTrace: " + ex.StackTrace);
            }

            Logger.Log("Connected to server");

            OnConnected?.Invoke();
        }

        public async Task ConnectToGameHub()
        {
            await GameHub.StartAsync();
        }

        public event Action OnConnected;

        public void On<T>(HubConnection connection, Action<T> callback) where T : INetworkMessage, new()
        {
            T temporaryMessage = new T();
            connection.On(temporaryMessage.MessageName, callback);
        }

        public async Task<T> SendAsync<T>(HubConnection connection, INetworkMessage message)
        {
            T result = await connection.InvokeAsync<T>(message.MessageName, message);
            return result;
        }

        public void Send(HubConnection connection, INetworkMessage message)
        {
            connection.SendAsync(message.MessageName, message);
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

        public async Task DisconnectFromGameHub()
        {
            await GameHub.StopAsync();
        }
    }
}