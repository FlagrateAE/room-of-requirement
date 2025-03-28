using UnityEngine;

public class SpellBookAnimationEventHandler : MonoBehaviour
{
    private SpellBookUI _bookUI;

    private void Start()
    {
        _bookUI = transform.parent.parent.GetComponentInChildren<SpellBookUI>();
    }

    public void ToggleUI() => _bookUI.ToggleUI();
}
