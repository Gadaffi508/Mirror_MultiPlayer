using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        public string displayName = "Loading...";

        private NetworkManagerLobby room;

        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null)
                    return room;
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);
            
            Room.gamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.gamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName) =>
            this.displayName = displayName;
    }
}
