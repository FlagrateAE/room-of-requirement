using System;
using UnityEngine;

/// <summary>
/// Represents a spell, including its form, controller type, power, and modifiers.
/// </summary>
[Serializable]
public class SpellData
{
    public readonly Forms Form;
    public readonly Type Controller;
    public readonly Color Color;
    public float Power { get; private set; }
    public float FlightSpeed { get; private set; } = 10f;


    public SpellData(Forms form, Type controller, Color color, float power)
    {
        Form = form;
        Controller = controller;
        Color = color;
        Power = power;
    }

    public void RegisterModifiers(string[] modifiersNames)
    {
        foreach (var modifier in modifiersNames)
        {
            switch (modifier)
            {
                case "Amplify":
                    Power *= ConfigManager.Instance.GetModifierFactor("Amplify");
                    break;

                case "Accelerate":
                case "Decelerate":
                    FlightSpeed *= ConfigManager.Instance.GetModifierFactor(modifier);
                    break;
            }
        }
    }
}
