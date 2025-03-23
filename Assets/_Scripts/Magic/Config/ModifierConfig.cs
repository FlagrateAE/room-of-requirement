using UnityEngine;

[CreateAssetMenu(fileName = "BaseModifierConfig", menuName = "Magic Configs/ModifierConfig", order = 2)]
public class ModifierConfig : BaseConfig
{
    [Header("Amplify")]
    public float AmplifyFactor = 1.5f;

    [Header("Accelerate")]
    public float AccelerateFactor = 1.5f;

    [Header("Decelerate")]
    public float DecelerateFactor = 0.5f;

    public float GetFactor(string modifierName) => GetValue<float>(modifierName);
}