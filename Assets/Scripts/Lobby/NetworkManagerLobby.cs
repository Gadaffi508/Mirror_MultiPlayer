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

        [Header("Room")] public NetworkRoomPlayerLobby roomPlayerPrefab = null;

        public static event Action onClientConnected;
        public static event Action onClientDisconnected;

        public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

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
            
            onClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            
            onClientDisconnected?.Invoke();
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

        public override void OnStopServer()
        {
            roomPlayers.Clear();
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
