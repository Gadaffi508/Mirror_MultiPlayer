using UnityEngine;

namespace Mirror
{
    public class NetworkGamePlayerWIT : NetworkBehaviour
    {
        [SyncVar]
        public string displayName = "Loading...";
        
        #region RoomInstance

        private NetworkManagerWIT room;

        private NetworkManagerWIT Room
        {
            get
            {
                if (room != null) return room;
                return room = NetworkManager.singleton as NetworkManagerWIT;
            }
        }

        #endregion

        public override void OnStartClient()
        {
            if(Room.dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            
            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }
    }
}
