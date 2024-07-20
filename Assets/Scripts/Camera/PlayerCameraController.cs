using System;
using System.Linq;
using Mirror.Inputs;
using UnityEngine;

namespace Mirror.Camera
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")] public Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        public Vector2 cameraVelocity = new Vector2(4f, 0.25f);
        public Transform playerTransform = null;
        public UnityEngine.Camera virtualCamera = null;

        private Controls _controls;

        private Controls Controls
        {
            get
            {
                if (_controls != null) return _controls;
                return _controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);

            enabled = true;

            Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();

        [ClientCallback]
        private void OnDisable() => Controls.Disable();

        private void Look(Vector2 readValue)
        {
            float deltaTime = Time.deltaTime;

            float followOffset = Mathf.Clamp(
                virtualCamera.transform.position.y - (readValue.y * cameraVelocity.y * deltaTime),
                maxFollowOffset.x,maxFollowOffset.y);
            
            Vector3 pos = virtualCamera.transform.position;

            pos.y = followOffset;

            virtualCamera.transform.position = pos;
            
            playerTransform.Rotate(0f, readValue.x * cameraVelocity.x * deltaTime, 0f);
        }
    }
}
