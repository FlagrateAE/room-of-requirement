using System.ComponentModel;
using UnityEngine;
using Zenject;

public class PlayerSpellCast : MonoBehaviour
{
    [Inject]
    private DiContainer _container;

    public void ArmSpell(SpellData spellData)
    {
        if (spellData == null) return;

        if (TryGetComponent<FormCaster>(out var formCaster))
            Destroy(formCaster);

        formCaster = gameObject.AddComponent(spellData.FormCaster) as FormCaster;
        _container.Inject(formCaster);
        formCaster.Initialize(spellData);
    }
}