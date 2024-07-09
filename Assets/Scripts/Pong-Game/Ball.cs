using System;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public float speed = 30f;
        
        public Rigidbody rigidbody;

        public override void OnStartServer()
        {
            base.OnStartServer();

            rigidbody.isKinematic = false;

            rigidbody.angularVelocity = Vector3.right * speed;
        }

        float HitFactor(Vector3 ballPos, Vector3 racketPos, float racketHeight)
        {
            return (ballPos.y - racketPos.y) / racketHeight;
        }

        [ServerCallback]
        void OnCollisionEnter(Collision other)
        {
            if (other.transform.GetComponent<Player>())
            {
                float y = HitFactor(transform.position,
                    other.transform.position,
                            other.collider.bounds.size.y);
                float x = other.relativeVelocity.x > 0 ? 1 : -1;

                Vector3 dir = new Vector3(x,0,y).normalized;

                rigidbody.angularVelocity = dir * speed;
            }
        }
    }
}
