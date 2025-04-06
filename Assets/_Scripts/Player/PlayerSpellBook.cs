using UnityEngine;
using System;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PlayerSpellCast), typeof(PlayerCamera), typeof(PlayerMovement))]
public class PlayerSpellBook : MonoBehaviour
{
    private PlayerSpellCast _spellCast;
    private PlayerMovement _movement;
    private PlayerCamera _camera;
    private CanvasGroup _bookCanvas;
    private Animator _bookAnimator;

    private bool _isOpen = false;

    private void Start()
    {
        _spellCast = GetComponent<PlayerSpellCast>();
        _movement = GetComponent<PlayerMovement>();
        _camera = GetComponent<PlayerCamera>();

        _bookAnimator = GetComponentInChildren<Animator>();
        _bookCanvas = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log("Creating Launch with no mods");
            _spellCast.CurrentSpell = CreateSpell(Forms.Projectile, "Launch");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Debug.Log("Creating Launch with 1 Amplify");
            _spellCast.CurrentSpell = CreateSpell(Forms.Projectile, "Launch", new string[] { "Amplify" });
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Debug.Log("Creating Launch with 3 Decelerate");
            _spellCast.CurrentSpell = CreateSpell(Forms.Projectile, "Launch", new string[] { "Decelerate", "Decelerate", "Decelerate" });
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Debug.Log("Creating Enlarge with no mods");
            _spellCast.CurrentSpell = CreateSpell(Forms.Projectile, "Enlarge");
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _bookAnimator.SetTrigger("BookToggle");

            if (_isOpen)
                ToggleUI();
        }
    }

    /// <summary>
    /// Creates a new spell with the specified form, effect, and optional modifiers.
    /// </summary>
    /// <param name="form">The form of the spell to create (e.g., Self, Projectile, MagicSphere).</param>
    /// <param name="effectName">The name of the effect to apply to the spell.</param>
    /// <param name="modifiersNames">Optional array of modifier names to register with the spell.</param>
    /// <returns>A new instance of <see cref="SpellData"/> configured with the specified parameters.</returns>
    private SpellData CreateSpell(Forms form, string effectName, string[] modifiersNames = null)
    {
        Type controller = Type.GetType($"{effectName}Controller");
        SpellData result = new(
            form,
            controller,
            ConfigManager.Instance.GetEffectColor(effectName),
            ConfigManager.Instance.GetEffectPower(effectName)
        );

        if (modifiersNames != null)
            result.RegisterModifiers(modifiersNames);

        return result;
    }

    public void ToggleUI()
    {
        _camera.ToggleCursorLock();
        _camera.ToggleCameraLock();
        _movement.ToggleMovment();
        _bookCanvas.alpha = Mathf.Abs(_bookCanvas.alpha - 1);
        _isOpen = !_isOpen;
    }
}
