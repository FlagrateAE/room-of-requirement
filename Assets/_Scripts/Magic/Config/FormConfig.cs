using UnityEngine;

[CreateAssetMenu(fileName = "BaseFormConfig", menuName = "Magic Configs/FormConfig", order = 0)]
public class FormConfig : BaseConfig
{
    [Header("Self")]
    public Texture2D SelfIcon;
    public string SelfDescription = "Applies the spell efect to the caster himself";

    [Header("Projectile")]
    public Texture2D ProjectileIcon;
    public string ProjectileDescription = "Sends a magic projectile that applies the spell effect to the target (first thing it hits)";

    [Header("Magic Sphere")]
    public Texture2D MagicSphereIcon;
    public string MagicSphereDescription = "Creates a magic sphere that applies the spell effect to all things inside it";
}