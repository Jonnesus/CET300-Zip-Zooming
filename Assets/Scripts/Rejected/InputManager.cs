using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public float move;
    [HideInInspector] public float steer;
    [HideInInspector] public float handbrake;
    [HideInInspector] public float boost;

    public void OnMove(InputValue value) => move = value.Get<Vector2>().y;
    public void OnSteer(InputValue value) => steer = value.Get<Vector2>().x;
    public void OnHandbrake(InputValue value) => handbrake = value.Get<float>();
    public void OnBoost(InputValue value) => boost = value.Get<float>();
}