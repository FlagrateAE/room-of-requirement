using UnityEngine;
using System.Collections.Generic;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class SpellInteractable : MonoBehaviour
{
    [Inject]
    private DiContainer _container;

    [SerializeField]
    private List<Effect> _incompatibleEffects = new();

    public bool IsCompatibleWith(Effect effect) => !_incompatibleEffects.Contains(effect);

    public void ApplySpell(SpellData spell)
    {
        var controller = (EffectController)gameObject.AddComponent(spell.EffectController);
        _container.Inject(controller);
        controller.Initialize(spell);
    }
}