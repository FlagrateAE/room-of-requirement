using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class FormCaster : MonoBehaviour
{
    protected InputAction _castInput;
    protected SpellData _spell;

    public virtual void Initialize(SpellData spell)
    {
        _castInput = InputSystem.actions.FindAction("Attack");
        _spell = spell;
    }

    private void Update()
    {
        if (_castInput.WasPressedThisFrame())
            Cast();
    }

    public abstract void Cast();
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
    [Inject(Id = "Projectile")]
    private GameObject _spellPrefab;
    private Transform _playerCamera;

    public override void Initialize(SpellData spell)
    {
        base.Initialize(spell);
        _playerCamera = Camera.main.transform;
    }

    public override void Cast()
    {
        Debug.Log(_spellPrefab);

        GameObject spellInstance = Instantiate(_spellPrefab, _playerCamera.position, _playerCamera.rotation);

        SpellProjectile spellComponent = spellInstance.GetComponent<SpellProjectile>();
        spellComponent.LoadSpellData(_spell);
    }
}