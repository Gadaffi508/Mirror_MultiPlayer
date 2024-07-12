using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class NetworkManagerWIT : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Room")] [SerializeField] private NetworkRoomPlayerWIT roomPlayerPrefab = null;

        [Header("Game")] [SerializeField] private NetworkGamePlayerWIT gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;
        [SerializeField] private GameObject roundSystem = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayerWIT> RoomPlayers { get; } = new List<NetworkRoomPlayerWIT>();
        public List<NetworkGamePlayerWIT> GamePlayers { get; } = new List<NetworkGamePlayerWIT>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("Player").ToList();

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("Player");

            foreach (var prefab in spawnablePrefabs)
            {
                //ClientScene.RegisterPrefab(prefab);
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

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkBehaviour>();

                if (player is NetworkRoomPlayerWIT roomPlayer)
                {
                    RoomPlayers.Remove(roomPlayer);

                    NotifyPlayersOfReadyState();
                }
                else if (player is NetworkGamePlayerWIT gamePlayer)
                {
                    GamePlayers.Remove(gamePlayer);
                }
            }
            
            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
            GamePlayers.Clear();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayerWIT roomPlayerWıt = Instantiate(roomPlayerPrefab);

                roomPlayerWıt.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerWıt.gameObject);
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Game"))
            {
                for (int i = RoomPlayers.Count -1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                    gamePlayerInstance.displayName = RoomPlayers[i].displayName;
                    
                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
        }

        public void StartGame()
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                if(!IsReadyToStart()) return;
                ServerChangeScene("Game");
            }
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) return false;
            foreach (var player in RoomPlayers)
            {
                if (!player.isReady) return false;
            }
            return true;
        }
    }
}
