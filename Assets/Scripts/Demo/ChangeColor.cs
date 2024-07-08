using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mirror
{
    public class ChangeColor : NetworkBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            transform.position = Vector3.zero + (Random.insideUnitSphere * 3f);
        }

        private void Update()
        {
            if(!base.isOwned) return;

            if (Input.GetKeyDown(KeyCode.C))
            {
                float r = Random.Range(0f, 1f);
                float b = Random.Range(0f, 1f);
                float g = Random.Range(0f, 1f);
                _spriteRenderer.color = new Color(r, g, b, 1f);
                CmdUpdateColor(_spriteRenderer.color);
            }
        }

        [Command]
        void CmdUpdateColor(Color c)
        {
            _spriteRenderer.color = c;
            RpcUpdateColor(c);
        }
        
        [ClientRpc]
        void RpcUpdateColor(Color c)
        {
            if(base.isServer) return;
            if(base.authority) return;

            _spriteRenderer.color = c;
        }
    }
}
