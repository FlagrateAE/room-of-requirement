using UnityEngine;
using System.Linq;
using System;

public class SpellInteractable : MonoBehaviour
{
    private readonly string[] _incompatibleEffects = { };

    public bool IsCompatibleWith(string effectName) => !_incompatibleEffects.Contains(effectName);

    /// <summary>
    /// Applies the spell to this SpellInteractable by adding and initializing the appropriate EffectController.
    /// </summary>
    /// <param name="spell">The SpellData representing the spell to apply, containing the controller type.</param>
    public void ApplySpell(SpellData spell)
    {
        var controller = (EffectController)gameObject.AddComponent(spell.Controller);
        controller.Initialize(spell);
    }
}