using UnityEngine;

public interface ISpellCaster
{
    public void ArmSpell(SpellData spellData);
    public void Cast();
    public Transform SpellSpawner { get; }
}