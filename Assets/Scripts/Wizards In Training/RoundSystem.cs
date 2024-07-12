using System;
using UnityEngine;

namespace Mirror
{
    public class RoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;
        
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

        public override void OnStartServer() => NetworkManagerWIT.OnServerReadied += (conn) => CheckToStartRound();

        [ServerCallback]
        void OnDestroy() => NetworkManagerWIT.OnServerReadied -= (conn) => CheckToStartRound();

        [Server]
        void CheckToStartRound()
        {
            //if (Room.GamePlayers.Count(x => x.isReaduy) != Room.GamePlayers.Count) return; 

            animator.enabled = true;

            RpcStartCountDown();
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        public void CountDownEnded()
        {
            animator.enabled = false;
        }

        [ClientRpc]
        void RpcStartCountDown()
        {
            animator.enabled = true;
        }

        [ClientRpc]
        void RpcStartRound()
        {
            // Input
        }
    }
}
