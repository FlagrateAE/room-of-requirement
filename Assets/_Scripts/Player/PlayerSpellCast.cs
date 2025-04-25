using UnityEngine;
using Zenject;

public class PlayerSpellCast : MonoBehaviour, ISpellCaster
{
    [Inject]
    private DiContainer _container;

    public Transform SpellSpawner { get; private set; }
    private FormCaster _formCaster;

    private void Start()
    {
        SpellSpawner = transform.Find("Main Camera");
    }

    public void ArmSpell(SpellData spellData)
    {
        if (spellData == null) return;
        if (_formCaster != null) Destroy(_formCaster);

        _formCaster = gameObject.AddComponent(spellData.FormCaster) as FormCaster;
        _container.Inject(_formCaster);
        _formCaster.Initialize(spellData);
    }

    public void Cast() => _formCaster.Cast();
}