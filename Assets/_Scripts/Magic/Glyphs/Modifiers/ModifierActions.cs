using System;
using System.Security;
using Zenject;

public static class ModifierActions
{
    [Inject]
    private static GlyphConfig _config;

    public static Action<SpellData> GetAction(Modifier modifier)
    {
        string methodName = Enum.GetName(typeof(Modifier), modifier);
        return (Action<SpellData>)
            typeof(ModifierActions)
            .GetMethod(methodName)
            .CreateDelegate(typeof(Action<SpellData>));
    }

    public static void Amplify(SpellData spell) => spell.Power *= 1.5f;
    public static void Accelerate(SpellData spell) => spell.FlightSpeed *= 1.5f;
    public static void Decelerate(SpellData spell) => spell.FlightSpeed *= 0.5f;
}