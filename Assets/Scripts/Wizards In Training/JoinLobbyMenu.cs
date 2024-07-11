using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerWIT networkManagerWıt = null;

        [Header("UI")] [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton;

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;

            networkManagerWıt.networkAddress = ipAddress;
            networkManagerWıt.StartClient();

            joinButton.interactable = false;
        }

        private void OnEnable()
        {
            NetworkManagerWIT.OnClientConnected += HandleClientConnect;
            NetworkManagerWIT.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerWIT.OnClientConnected -= HandleClientConnect;
            NetworkManagerWIT.OnClientDisconnected -= HandleClientDisconnected;
        }

        void HandleClientConnect()
        {
            joinButton.interactable = true;
            
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}
