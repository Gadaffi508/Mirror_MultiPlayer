using System;
using System.Collections.Generic;
using UnityEngine;

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

        //public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("Player");

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
            if (!clientLoadedScene)
            {
                //if(ClientScene.ready) ClientScene.Ready(conn);
                //ClientScene.AddPlayer();
            }
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

        public void StartGame()
        {
            
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
