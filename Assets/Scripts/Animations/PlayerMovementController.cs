using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mirror.Movement
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [Header("References")] public CharacterController controller = null;
        public Animator animator = null;
        
        [Header("Settings")] public float movementSpeed = 2.5f;

        [ClientCallback]
        void Update()
        {
            if(!isLocalPlayer) return;

            var movement = new Vector3();

            if (Keyboard.current.wKey.isPressed) movement.z += 1;
            if (Keyboard.current.sKey.isPressed) movement.z -= 1;
            if (Keyboard.current.aKey.isPressed) movement.x -= 1;
            if (Keyboard.current.dKey.isPressed) movement.x += 1;

            controller.Move(movement * movementSpeed * Time.deltaTime);
            
            if(controller.velocity.magnitude > 0.2f)
                transform.rotation = Quaternion.LookRotation(movement);
            
            animator.SetBool("IsWalking",controller.velocity.magnitude > 0.2f);
        }
    }
}
