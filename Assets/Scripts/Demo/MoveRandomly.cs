using System.Collections;
using UnityEngine;

namespace Mirror
{
    public class MoveRandomly : NetworkBehaviour
    {
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            StartCoroutine(__Move());
        }

        IEnumerator __Move()
        {
            while (true)
            {
                transform.position = Vector3.zero + (Random.insideUnitSphere * 3f);
                yield return new WaitForSeconds(2f);
            }
        }
    }
}
