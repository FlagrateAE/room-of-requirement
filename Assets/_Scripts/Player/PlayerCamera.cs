using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;

    private InputAction _mouseInput;
    public float Sensitivity = 5f;

    private Vector2 _mouseDelta;
    private float _pitch;


    private void Start()
    {
        _mouseInput = InputSystem.actions.FindAction("Look");
        ToggleCursorLock();
    }

    private void Update()
    {
        _mouseDelta = _mouseInput.ReadValue<Vector2>();

        transform.Rotate(Vector3.up, _mouseDelta.x * Sensitivity * Time.deltaTime);

        _pitch += _mouseDelta.y * Sensitivity * Time.deltaTime;
        _pitch = Math.Clamp(_pitch, -90, 90);
        _camera.localRotation = Quaternion.AngleAxis(_pitch, Vector3.left);
    }

    public void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}