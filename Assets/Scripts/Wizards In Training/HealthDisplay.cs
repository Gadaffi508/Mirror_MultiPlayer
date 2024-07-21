using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class HealthDisplay : MonoBehaviour
    {
        [Header("References")] public Health health = null;
        public Image healthBarImage = null;

        private void OnEnable()
        {
            health.EventHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            health.EventHealthChanged -= HandleHealthChanged;
        }

        void HandleHealthChanged(int currentHealth, int maxHealth)
        {
            
        }
    }
}
