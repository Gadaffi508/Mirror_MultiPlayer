using UnityEngine;

namespace Optimization.DC
{
    public class ClassicMovement : MonoBehaviour
    {
        public float speed = 10f;

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
