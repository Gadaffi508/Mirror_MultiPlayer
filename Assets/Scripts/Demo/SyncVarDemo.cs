using System.Collections;
using UnityEngine;

namespace Mirror
{
    public class SyncVarDemo : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetColor))]
        private Color32 _color = Color.red;

        public override void OnStartServer()
        {
            base.OnStartServer();
            StartCoroutine(__RandomizeColor());
        }

        void SetColor(Color32 oldCOlor, Color32 newColor)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = newColor;
        }

        IEnumerator __RandomizeColor()
        {
            WaitForSeconds wait = new WaitForSeconds(2f);
            while (true)
            {
                yield return wait;

                _color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f, 1f, 1f);
            }
        }
    }
}
