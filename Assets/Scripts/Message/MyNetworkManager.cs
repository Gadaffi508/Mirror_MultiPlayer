using Mirror.Message;
using UnityEngine;

namespace Mirror
{
    public class MyNetworkManager : NetworkManager
    {
        public string notificationMessage = string.Empty;
        public override void OnStartServer()
        {
            ServerChangeScene("Game");
        }

        [ContextMenu("Send Notification")]
        void SendNotification()
        {
            NetworkServer.SendToAll(new Notification { content = notificationMessage});
        }
    }
}
