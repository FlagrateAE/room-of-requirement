using UnityEngine;
using System;

[RequireComponent(typeof(PlayerSpellCast))]
public class PlayerSpellBook : MonoBehaviour
{
    [SerializeField]
    private Animator _bookAnimator;
    [SerializeField]
    private EffectConfig _effectConfig;
    [SerializeField]
    private ModifierConfig _modifierConfig;
    private PlayerSpellCast _spellCast;

    private bool _isOpen;

    private void Start()
    {
        _spellCast = GetComponent<PlayerSpellCast>();
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
        }
    }

    private void LoadGlyphData(FormConfig formConfig, EffectConfig effectConfig, ModifierConfig modifierConfig)
    {
        
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
            _effectConfig.GetColor(effectName),
            _effectConfig.GetPower(effectName)
        );

        if (modifiersNames != null)
            result.RegisterModifiers(modifiersNames, _modifierConfig);

        return result;
    }
}
