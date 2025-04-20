using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellData
{
    public Form Form;
    public Type FormCaster;
    public Effect Effect;
    public Type EffectController;
    public Color Color;
    public float Power;
    public float FlightSpeed = 10f;
    public List<Modifier> Modifiers = new();

    // MODIFIERS DATA
    public int PierceDepth = 0;

    public override string ToString() => $"{Form} {EffectController.Name} {Color} {Power} {FlightSpeed}";
}
