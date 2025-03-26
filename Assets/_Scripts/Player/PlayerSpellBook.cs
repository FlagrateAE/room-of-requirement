using UnityEngine;
using System;

[RequireComponent(typeof(PlayerSpellCast), typeof(PlayerCamera), typeof(PlayerMovement))]
public class PlayerSpellBook : MonoBehaviour
{
    private PlayerSpellCast _spellCast;
    private PlayerMovement _playerMovement;
    private PlayerCamera _playerCamera;

    [SerializeField]
    private GameObject _spellBook;
    [SerializeField]
    private SpellBookUI _bookUI;
    private Animator _bookAnimator;

    private bool _isOpen;

    private void Start()
    {
        _spellCast = GetComponent<PlayerSpellCast>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCamera = GetComponent<PlayerCamera>();
        _bookAnimator = _spellBook.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log("Creating Launch with no mods");
            _spellCast.CurrentSpell = CreateSpell(SpellForm.Projectile, "Launch");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Debug.Log("Creating Launch with 1 Amplify");
            _spellCast.CurrentSpell = CreateSpell(SpellForm.Projectile, "Launch", new string[] { "Amplify" });
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Debug.Log("Creating Launch with 3 Decelerate");
            _spellCast.CurrentSpell = CreateSpell(SpellForm.Projectile, "Launch", new string[] { "Decelerate", "Decelerate", "Decelerate" });
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Debug.Log("Creating Enlarge with no mods");
            _spellCast.CurrentSpell = CreateSpell(SpellForm.Projectile, "Enlarge");
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _bookAnimator.SetTrigger("BookToggle");

            if (_isOpen)
                _playerCamera.LockCursor();

            _playerMovement.enabled = _playerCamera.enabled = _isOpen;
            _isOpen = !_isOpen;
        }
    }

    /// <summary>
    /// Creates a new spell with the specified form, effect, and optional modifiers.
    /// </summary>
    /// <param name="form">The form of the spell to create (e.g., Self, Projectile, MagicSphere).</param>
    /// <param name="effectName">The name of the effect to apply to the spell.</param>
    /// <param name="modifiersNames">Optional array of modifier names to register with the spell.</param>
    /// <returns>A new instance of <see cref="SpellData"/> configured with the specified parameters.</returns>
    private SpellData CreateSpell(SpellForm form, string effectName, string[] modifiersNames = null)
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
}
