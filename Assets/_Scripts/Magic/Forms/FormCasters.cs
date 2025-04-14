using UnityEngine;

public abstract class FormCaster : MonoBehaviour
{
    private SpellData _spell;
    public void Initialize(SpellData spell) => _spell = spell;
    public abstract void Cast();
}

public class SelfCaster : FormCaster
{
    public override void Cast()
    {
        
    }
}
