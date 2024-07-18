using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")] public GameObject lobbyUI = null;
        public Text[] playerNameText, playerReadyText = new Text[4];
        public Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";

        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;

        private bool İsLeader = false;

        public bool IsLeader
        {
            set
            {
                İsLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

        private NetworkManagerLobby room;

        public NetworkManagerLobby Room
        {
            get
            {
                if (room != null) return room;
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.name);

            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.roomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.roomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyToStart(bool ısReady)
        {
            if(!İsLeader) return;

            startGameButton.interactable = ısReady;
        }

        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

        void UpdateDisplay()
        {
            if (!isLocalPlayer)
            {
                foreach (var player in Room.roomPlayers)
                {
                    if (player.isLocalPlayer)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }

                return;
            }

            for (int i = 0; i < playerNameText.Length; i++)
            {
                playerNameText[i].text = "Waiting For player...";
                playerReadyText[i].text = string.Empty;
            }

            for (int i = 0; i < Room.roomPlayers.Count; i++)
            {
                playerNameText[i].text = Room.roomPlayers[i].DisplayName;
                playerReadyText[i].text = Room.roomPlayers[i].IsReady
                    ? "<color = green> Ready </color>"
                    : "<color = red> UnReady </color>";
            }
        }
        
        //<--------------------------------------------------Commands-------------------------------------------------->

        [Command]
        void CmdSetDisplayName(string name)
        {
            DisplayName = name;
        }

        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;
            
            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            if(Room.roomPlayers[0].connectionToClient != connectionToClient) return;
            
            Room.StartGame();
        }
    }
}