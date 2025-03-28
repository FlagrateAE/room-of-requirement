using UnityEngine;

[CreateAssetMenu(fileName = "BaseModifierConfig", menuName = "Magic Configs/ModifierConfig", order = 2)]
public class ModifierConfig : BaseConfig
{
    [Header("Amplify")]
    public float AmplifyFactor = 1.5f;
    public Sprite AmplifyIcon;
    public string AmplifyDescription = "Increases the power of the spell effect";

    [Header("Accelerate")]
    public float AccelerateFactor = 1.5f;
    public Sprite AccelerateIcon;
    public string AccelerateDescription = "Increases the speed of the spell projectile";

    [Header("Decelerate")]
    public float DecelerateFactor = 0.5f;
    public Sprite DecelerateIcon;
    public string DecelerateDescription = "Decreases the speed of the spell projectile";

    public float GetFactor(string modifierName) => GetValue<float>(modifierName);
}