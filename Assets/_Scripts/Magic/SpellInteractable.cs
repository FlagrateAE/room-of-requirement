using UnityEngine;
using System.Linq;
using System;

public class SpellInteractable : MonoBehaviour
{
    private readonly string[] _incompatibleEffects = { };

    public bool IsCompatibleWith(string effectName) => !_incompatibleEffects.Contains(effectName);

    public void ApplySpell(SpellData spell)
    {
        var controller = (EffectController)gameObject.AddComponent(spell.Controller);
        controller.Initialize(spell);
    }
}