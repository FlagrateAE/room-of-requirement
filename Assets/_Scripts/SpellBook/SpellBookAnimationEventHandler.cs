using UnityEngine;

public class SpellBookAnimationEventHandler : MonoBehaviour
{
    private PlayerSpellBook _playerSpellBook;

    private void Start()
    {
        _playerSpellBook = GetComponentInParent<PlayerSpellBook>();
    }

    public void ToggleUI() => _playerSpellBook.ToggleUI();
}
