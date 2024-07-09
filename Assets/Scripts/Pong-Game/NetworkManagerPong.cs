using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class NetworkManagerPong : NetworkManager
    {
        public Transform leftRacketSpawn,rightRacketSpawn;

        private GameObject ball;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(playerPrefab => playerPrefab.name == "Ball"));
                NetworkServer.Spawn(ball);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if(ball != null)
                NetworkServer.Destroy(ball);

            base.OnServerDisconnect(conn);
            
            
        }
    }
}
