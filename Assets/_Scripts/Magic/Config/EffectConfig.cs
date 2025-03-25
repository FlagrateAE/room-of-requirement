using UnityEngine;

[CreateAssetMenu(fileName = "BaseEffectConfig", menuName = "Magic Configs/EffectConfig", order = 1)]
public class EffectConfig : BaseConfig
{
    [Header("Launch")]
    public Color LaunchColor = Color.white;
    public float LaunchForce = 10f;
    public Sprite LaunchIcon;
    public string LaunchDescription = "Sends the object high up in the air";


    [Header("Enlarge")]
    public Color EnlargeColor = Color.red;
    public float EnlargeFactor = 1.5f;
    public Sprite EnlargeIcon;
    public string EnlargeDescription = "Makes object larger. There is a size limit";

    public float GetPower(string effectName) => GetValue<float>(effectName);
    public Color GetColor(string effectName) => GetValue<Color>(effectName);
}