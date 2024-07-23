using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class PlayerInfoDisplay : NetworkBehaviour
    {
        [SyncVar (hook = nameof(HandleSteamIDUpdated))] 
        private ulong steamID;
        
        public RawImage playerImage = null;
        public TMP_Text displayNameText = null;

        protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

        #region Server

        public void SetSteamID(ulong steamId)
        {
            this.steamID = steamId;
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
        }

        void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
        {
            if(callback.m_steamID.m_SteamID != steamID) return;

            playerImage.texture = GetSteamImageAsTexture(callback.m_iImage);
        }

        void HandleSteamIDUpdated(ulong oldSteamID, ulong newSteamID)
        {
            var csSteamId = new CSteamID(newSteamID);

            displayNameText.text = SteamFriends.GetFriendPersonaName(csSteamId);

            int imageId = SteamFriends.GetLargeFriendAvatar(csSteamId);
            
            if(imageId == -1) return;

            playerImage.texture = GetSteamImageAsTexture(imageId);
        }

        Texture2D GetSteamImageAsTexture(int iImage)
        {
            Texture2D texture = null;

            bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

            if (isValid)
            {
                byte[] image = new byte[width * height * 4];
                
                isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

                if (isValid)
                {
                    texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32,false,true);
                    texture.LoadRawTextureData(image);
                    texture.Apply();
                }
            }
            
            return texture;
        }

        #endregion

        
    }
}
