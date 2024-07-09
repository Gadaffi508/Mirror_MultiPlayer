using System;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody rigidbody;

        private void FixedUpdate()
        {
            if (isLocalPlayer)
                rigidbody.linearVelocity = new Vector3(0,Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        }
    }
}
