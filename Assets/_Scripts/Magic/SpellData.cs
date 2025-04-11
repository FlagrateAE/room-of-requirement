using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellData
{
    public readonly Form Form;
    public readonly Effect Effect;
    public readonly Type Controller;
    public readonly Color Color;
    public float Power { get; private set; }
    public float FlightSpeed { get; private set; } = 10f;

    public SpellData(Form form, Effect effect, Type controller, Color color, float power)
    {
        Form = form;
        Effect = effect;
        Controller = controller;
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

    public override string ToString() => $"{Form} {Controller.Name} {Color} {Power} {FlightSpeed}";
}
