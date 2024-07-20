using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mirror
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;

        private static List<Transform> _spawnPoints = new List<Transform>();

        private int nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            _spawnPoints.Add(transform);

            _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

        public override void OnStartClient()
        {
            //Input Managers
        }

        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;


        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = _spawnPoints.ElementAtOrDefault(nextIndex);
            
            if(spawnPoint == null) return;

            GameObject playerInstance = Instantiate(playerPrefab, _spawnPoints[nextIndex].position,
                _spawnPoints[nextIndex].rotation);
            
            NetworkServer.Spawn(playerInstance,conn);

            nextIndex++;
        }

        [ServerCallback]
        private void OnDestroy()
        {
            if(!isServer) return;
            
            NetworkManagerLobby.OnServerReadied -= SpawnPlayer;
        }
    }
}
