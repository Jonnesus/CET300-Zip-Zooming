using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public float move;
    [HideInInspector] public float steer;

    public void OnMove(InputValue value) => move = value.Get<Vector2>().y;
    public void OnSteer(InputValue value) => steer = value.Get<Vector2>().x;
}