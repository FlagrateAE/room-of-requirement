using UnityEngine;
using System.Collections.Generic;

public class SpellInteractable : MonoBehaviour
{
    [SerializeField]
    private List<Effect> _incompatibleEffects = new();

    public bool IsCompatibleWith(Effect effect) => !_incompatibleEffects.Contains(effect);

    public void ApplySpell(SpellData spell)
    {
        var controller = (EffectController)gameObject.AddComponent(spell.Controller);
        controller.Initialize(spell);
    }
}