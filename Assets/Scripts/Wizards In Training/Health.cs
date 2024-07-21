using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mirror
{
    public class Health : NetworkBehaviour
    {
        [Header("Settings")] public int maxHealth = 100;
        public int damagePerPress = 100;

        [SyncVar] public int currentHealth;

        public delegate void HealthChangedDelegate(int currentHealth, int maxHealth);

        public event HealthChangedDelegate EventHealthChanged;

        #region Server

        [Server]
        private void SetHealth(int value)
        {
            currentHealth = value;
            EventHealthChanged?.Invoke(currentHealth,maxHealth);
        }

        public override void OnStartServer() => SetHealth(maxHealth);

        [Command]
        void CmdDealDamage() => SetHealth(Mathf.Max(currentHealth - damagePerPress, 0));

        #endregion

        #region Client

        [ClientCallback]
        void Update()
        {
            if(!isLocalPlayer) return;
            
            if(!Keyboard.current.spaceKey.wasPressedThisFrame) return;
            
            CmdDealDamage();
        }

        #endregion
    }
}
