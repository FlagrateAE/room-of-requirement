using UnityEngine;
using Zenject;

public abstract class EffectController : MonoBehaviour
{
    [Inject]
    protected GlyphConfig _config;

    protected float _power;
    protected Form _form;

    public void Initialize(SpellData spell)
    {
        _form = spell.Form;
        _power = spell.Power;

        foreach (var modifier in spell.Modifiers)
            RegisterModifier(modifier);

        ApplyEffect();
        Destroy(this);
    }

    public virtual void RegisterModifier(Modifier modifier)
    {
        switch (modifier)
        {
            case Modifier.Amplify:
            case Modifier.Dampen:
                _power *= _config.GetFactor(modifier);
                break;
        }
    }

    public abstract void ApplyEffect();
}

[RequireComponent(typeof(Rigidbody))]
public class LaunchController : EffectController
{
    public override void ApplyEffect()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * _power, ForceMode.Impulse);
    }
}

public class ResizeController : EffectController
{
    public override void ApplyEffect()
    {
        transform.localScale *= _power;
    }
}
