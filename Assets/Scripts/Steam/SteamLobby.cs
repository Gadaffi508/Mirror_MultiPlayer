using System;
using Steamworks;
using UnityEngine;

namespace Mirror
{
    public class SteamLobby : MonoBehaviour
    {
        public GameObject buttons = null;
        
        public static CSteamID lobbyID { get; private set; }

        private NetworkManager _networkManager = null;

        private const string HostAdressKey = "HostAdress";

        protected Callback<LobbyCreated_t> lobbyCreated = null;

        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested = null;

        protected Callback<LobbyEnter_t> lobbyEntered;

        private void Start()
        {
            _networkManager = GetComponent<NetworkManager>();
            
            if(!SteamManager.Initialized) return;

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            buttons.SetActive(false);

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _networkManager.maxConnections);
        }

        void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                buttons.SetActive(true);
                return;
            }
            
            lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            
            _networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(lobbyID, HostAdressKey,
                SteamUser.GetSteamID().ToString());
        }

        void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        void OnLobbyEntered(LobbyEnter_t callback)
        {
            if(NetworkServer.active) return;

            string hostAdress = SteamMatchmaking.GetLobbyData(new CSteamID
                (callback.m_ulSteamIDLobby), HostAdressKey);

            _networkManager.networkAddress = hostAdress;
            
            _networkManager.StartClient();
            
            buttons.SetActive(false);
        }
    }
}
