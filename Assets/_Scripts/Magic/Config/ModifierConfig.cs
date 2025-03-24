using UnityEngine;

[CreateAssetMenu(fileName = "BaseModifierConfig", menuName = "Magic Configs/ModifierConfig", order = 2)]
public class ModifierConfig : BaseConfig
{
    [Header("Amplify")]
    public float AmplifyFactor = 1.5f;
    public Sprite AmplifyIcon;

    [Header("Accelerate")]
    public float AccelerateFactor = 1.5f;
    public Sprite AccelerateIcon;

    [Header("Decelerate")]
    public float DecelerateFactor = 0.5f;
    public Sprite DecelerateIcon;

    public float GetFactor(string modifierName) => GetValue<float>(modifierName);
}