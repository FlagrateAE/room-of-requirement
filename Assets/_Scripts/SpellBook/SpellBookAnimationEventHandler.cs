using UnityEngine;

public class SpellBookAnimationEventHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerCamera _playerCamera;

    public void UnlockCursor() => _playerCamera.UnlockCursor();
}
