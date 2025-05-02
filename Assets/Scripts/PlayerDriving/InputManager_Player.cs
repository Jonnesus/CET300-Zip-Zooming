using UnityEngine;

namespace ArcadeVP
{
    public class InputManager_Player : MonoBehaviour
    {
        public PlayerVehicleController playerVehicleController;

        [HideInInspector] public float Horizontal;
        [HideInInspector] public float Vertical;
        [HideInInspector] public float Jump;

        private void Update()
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            Jump = Input.GetAxis("Jump");

            playerVehicleController.ProvideInputs(Horizontal, Vertical, Jump);
        }
    } 
}