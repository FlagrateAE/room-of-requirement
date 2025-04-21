using UnityEngine;

[RequireComponent(typeof(PlayerSpellCast), typeof(PlayerCamera), typeof(PlayerMovement))]
public class PlayerSpellBook : MonoBehaviour
{
    private PlayerSpellCast _spellCast;
    private PlayerMovement _movement;
    private PlayerCamera _camera;
    private CanvasGroup _bookCanvas;
    private Animator _bookAnimator;
    private SpellBook _spellBook;

    private bool _isOpen = false;

    private void Start()
    {
        _spellCast = GetComponent<PlayerSpellCast>();
        _movement = GetComponent<PlayerMovement>();
        _camera = GetComponent<PlayerCamera>();

        _spellBook = GetComponentInChildren<SpellBook>();
        _bookAnimator = GetComponentInChildren<Animator>();
        _bookCanvas = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            _bookAnimator.SetTrigger("BookToggle");

            if (_isOpen)
            {
                ToggleUI();
                _spellCast.ArmSpell(_spellBook.Builder.BuildSpell());
            }
        }
    }

    public void ToggleUI()
    {
        if (TryGetComponent<FormCaster>(out var formCaster))
        {
            formCaster.ToggleArmed();
        }

        _camera.ToggleCursorLock();
        _camera.ToggleCameraLock();
        _movement.ToggleMovment();
        _bookCanvas.alpha = Mathf.Abs(_bookCanvas.alpha - 1);
        _isOpen = !_isOpen;
    }
}
