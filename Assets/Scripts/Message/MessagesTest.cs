using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Message
{
    public class MessagesTest : MonoBehaviour
    {
        public Text notificationText = null;

        private void Start()
        {
            if (!NetworkClient.active)
            {
                NetworkClient.RegisterHandler<Notification>(OnNotification);
            }
        }

        void OnNotification(Notification msg, int channelId)
        {
            notificationText.text += $"\n{msg.content}";
        }
    }
    
    public struct Notification : NetworkMessage
    {
        public string content;
    }
}
