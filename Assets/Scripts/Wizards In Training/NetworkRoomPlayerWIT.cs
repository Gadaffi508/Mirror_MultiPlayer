using UnityEngine.UI;
using UnityEngine;

namespace Mirror
{
    public class NetworkRoomPlayerWIT : NetworkRoomPlayer
    {
        [Header("UI")] [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private Text[] playerNameTexts = new Text[4];
        [SerializeField] private Text[] playerReadyTexts = new Text[4];
        [SerializeField] private Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string displayName = "Loading...";
        
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool isReady = false;

        #region IsLeader

        private bool _isLeader;

        public bool IsLeader
        {
            set
            {
                _isLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

        #endregion

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

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.name);
            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);
            
            UpdateDisplay();
        }
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

        void UpdateDisplay()
        {
            if (!isLocalPlayer)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.isLocalPlayer)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting For Player...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].displayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].isReady ? 
                    "<color = green> Ready </color>" : 
                    "<color = red> UnReady </color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if(!_isLeader) return;

            startGameButton.interactable = readyToStart;
        }


        [Command]
        void CmdSetDisplayName(string _displayName)
        {
            this.displayName = _displayName;
        }
        
        [Command]
        public void CmdReadyUp()
        {
            isReady = !isReady;

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            Room.StartGame();
        }
    }
}
