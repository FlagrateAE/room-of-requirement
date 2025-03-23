using UnityEngine;

public abstract class EffectController : MonoBehaviour
{
    protected float _power;
    protected SpellForm _form;

    public void Initialize(SpellData spell)
    {
        _form = spell.Form;
        _power = spell.Power;

        ApplyEffect();
    }

    public abstract void ApplyEffect();
}

public class LaunchController : EffectController
{
    public override void ApplyEffect()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * _power, ForceMode.Impulse);
    }
}

public class EnlargeController : EffectController
{
    public override void ApplyEffect()
    {
        transform.localScale *= _power;
    }
}