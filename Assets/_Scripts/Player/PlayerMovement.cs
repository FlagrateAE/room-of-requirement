using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private InputAction _movementInput;
    private Vector2 _movement;

    public float Speed = 3f;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _movementInput = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        _movement = _movementInput.ReadValue<Vector2>() * Speed;

        _rb.linearVelocity = transform.rotation * new Vector3(
            _movement.x,
            _rb.linearVelocity.y,
            _movement.y
        );
    }

    public void ToggleMovment()
    {
        enabled = !enabled;
    }
}
