using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellData
{
    public readonly Form Form;
    public readonly Type FormCaster;
    public readonly Effect Effect;
    public readonly Type EffectController;
    public readonly Color Color;
    public float Power { get; private set; }
    public float FlightSpeed { get; private set; } = 10f;

    public SpellData(Form form, Type formCaster, Effect effect, Type effectController, Color color, float power)
    {
        Form = form;
        FormCaster = formCaster;
        Effect = effect;
        EffectController = effectController;
        Color = color;
        Power = power;
    }

    [Inject]
    public void RegisterModifiers(List<Modifier> modifiers, GlyphConfig config)
    {
        foreach (var modifier in modifiers)
        {
            switch (modifier)
            {
                case Modifier.Amplify:
                    Power *= config.GetFactor(modifier);
                    break;

                case Modifier.Accelerate:
                case Modifier.Decelerate:
                    FlightSpeed *= config.GetFactor(modifier);
                    break;
            }
        }
    }

    public override string ToString() => $"{Form} {EffectController.Name} {Color} {Power} {FlightSpeed}";
}
