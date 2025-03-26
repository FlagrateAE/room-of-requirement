using UnityEngine;

public class SpellBookAnimationEventHandler : MonoBehaviour
{
    private PlayerCamera _playerCamera;

    private void Start()
    {
        _playerCamera = transform.GetComponentInParent<PlayerCamera>();
    }

    public void UnlockCursor() => _playerCamera.UnlockCursor();
}
