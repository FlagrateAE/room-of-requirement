using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseModifierConfig", menuName = "Magic Configs/ModifierConfig", order = 2)]
public class ModifierConfig : BaseConfig
{
    [Header("Amplify")]
    public float AmplifyFactor = 1.5f;
    public Sprite AmplifyIcon;
    public string AmplifyDescription = "Increases the power of the spell effect";
    public string[] AmplifyCompatibles = { "Launch", "Enlarge" };

    [Header("Accelerate")]
    public float AccelerateFactor = 1.5f;
    public Sprite AccelerateIcon;
    public string AccelerateDescription = "Increases the speed of the spell projectile";
    public string[] AccelerateCompatibles = { "Projectile" };

    [Header("Decelerate")]
    public float DecelerateFactor = 0.5f;
    public Sprite DecelerateIcon;
    public string DecelerateDescription = "Decreases the speed of the spell projectile";
    public string[] DecelerateCompatibles = { "Projectile" };

    public float GetFactor(string modifierName) => GetValue<float>(modifierName);
    public bool IsCompatible(string modifierName, string glyphName)
    {
        string[] compatibles = GetValue<string[]>(modifierName);
        foreach (string compatible in compatibles)
        {
            if (glyphName == compatible) return true;
        }

        return false;
    }
}