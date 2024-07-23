using Steamworks;
using UnityEngine;

namespace Mirror
{
    public class SteamNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.lobbyID, numPlayers - 1);

            var playerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();
            
            playerInfoDisplay.SetSteamID(steamID.m_SteamID);
        }
    }
}
