using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class FormCaster : MonoBehaviour
{
    [Inject]
    protected GlyphConfig _config;
    protected InputAction _castInput;
    protected SpellData _spell;
    protected bool _isArmed;

    public virtual void Initialize(SpellData spell)
    {
        _castInput = InputSystem.actions.FindAction("Attack");
        _spell = spell;

        foreach (var modifier in spell.Modifiers)
            RegisterModifier(modifier);

        _isArmed = true;
    }

    private void Update()
    {
        if (_castInput != null && _isArmed && _castInput.WasPressedThisFrame())
        {
            Cast();
        }
    }

    public abstract void Cast();
    public virtual void RegisterModifier(Modifier modifier) { }

    public void ToggleArmed() => _isArmed = !_isArmed;
}

[RequireComponent(typeof(SpellInteractable))]
public class SelfCaster : FormCaster
{
    public override void Cast()
    {
        var interactable = GetComponent<SpellInteractable>();

        if (interactable.IsCompatibleWith(_spell.Effect))
            interactable.ApplySpell(_spell);
    }
}

public class ProjectileCaster : FormCaster
{
    [Inject(Id = Form.Projectile)]
    private GameObject _spellPrefab;
    private Transform _spellSpawner;

    public override void Initialize(SpellData spell)
    {
        base.Initialize(spell);
        _spellSpawner = GetComponentInParent<ISpellCaster>().SpellSpawner;
    }

    public override void RegisterModifier(Modifier modifier)
    {
        switch (modifier)
        {
            case Modifier.Pierce:
                _spell.PierceDepth++;
                break;
        }
    }

    public override void Cast()
    {
        GameObject spellInstance = Instantiate(_spellPrefab, _spellSpawner.position, _spellSpawner.rotation);

        SpellProjectile spellComponent = spellInstance.GetComponent<SpellProjectile>();
        spellComponent.LoadSpellData(_spell);
    }
}