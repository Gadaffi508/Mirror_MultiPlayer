using UnityEngine;

namespace Mirror
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerWIT networkManagerWıt = null;

        [Header("UI")] [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManagerWıt.StartHost();
            
            landingPagePanel.SetActive(false);
        }
    }
}
