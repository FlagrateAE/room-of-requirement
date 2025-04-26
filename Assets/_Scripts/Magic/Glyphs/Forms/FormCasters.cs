using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
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

public class ProjectileCaster : FormCaster, IFormCasterPooler
{
    [Inject(Id = Form.Projectile)]
    private GameObject _spellPrefab;
    private Transform _spellSpawner;

    private ObjectPool<GameObject> _pool;

    public override void Initialize(SpellData spell)
    {
        base.Initialize(spell);
        _spellSpawner = GetComponentInParent<ISpellCaster>().SpellSpawner;

        _pool = new ObjectPool<GameObject>(
            createFunc: () =>
                CreateSpell(),
            actionOnGet: spellInstance =>
                spellInstance.GetComponent<SpellProjectile>().OnGetFromPool(_spellSpawner),
            actionOnRelease: spellInstance =>
                spellInstance.GetComponent<SpellProjectile>().OnReleaseToPool(),
            actionOnDestroy: spellInstance =>
                Destroy(spellInstance),
            collectionCheck: true
        );
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
        GameObject spellInstance = _pool.Get();
        spellInstance.transform.SetPositionAndRotation(_spellSpawner.position, _spellSpawner.rotation);
    }

    public GameObject CreateSpell()
    {
        GameObject spellInstance = Instantiate(_spellPrefab);

        SpellProjectile spellComponent = spellInstance.GetComponent<SpellProjectile>();
        spellComponent.LoadSpellData(_spell, _pool);

        return spellInstance;
    }
}

public interface IFormCasterPooler
{
    GameObject CreateSpell();
}