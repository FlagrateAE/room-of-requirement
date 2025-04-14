using UnityEngine;
using System;

public class EffectGlyph : Glyph
{
    public Color Color;
    public float Power;
    public Type Controller;
}

public enum Effect
{
    Launch,
    Enlarge
}