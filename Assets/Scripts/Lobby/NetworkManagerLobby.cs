using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class NetworkManagerLobby : NetworkManager
    {
        public int minPlayers = 2;
        [Scene] public string menuScene = string.Empty;
        [Scene] public string gameScene = string.Empty;

        [Header("Room")] public NetworkRoomPlayerLobby roomPlayerPrefab = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied; 

        [Header("Game")] public NetworkGamePlayerLobby gamePlayerLobby;
        public GameObject playerSpawnSystem = null;

        public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
        public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("Player").ToList();

        public override void OnStartClient()
        {
            var spawnablePrefab = Resources.LoadAll<GameObject>("Player");

            foreach (var prefab in spawnablePrefab)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            if (numPlayers >= minPlayers)
            {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().name != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if(SceneManager.GetActiveScene().name != menuScene) return;

            bool isLeader = roomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                roomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }
            
            base.OnServerDisconnect(conn);
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith(gameScene))
            {
                for (int i = roomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = roomPlayers[i].connectionToClient;
                    var gamePlayerInstance = Instantiate(gamePlayerLobby);
                    gamePlayerInstance.SetDisplayName(roomPlayers[i].DisplayName);
                    
                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
                }
            }
            
            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            if (newSceneName.StartsWith(gameScene))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            
            OnServerReadied?.Invoke(conn);
        }

        public override void OnStopServer()
        {
            roomPlayers.Clear();
        }

        public void StartGame()
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                if(!IsReadyToStart()) return;
                
                ServerChangeScene(gameScene);
            }
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in roomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) return false;

            foreach (var player in roomPlayers)
            {
                if (!player.IsReady) return false;
            }
            
            return true;
        }
    }
}
