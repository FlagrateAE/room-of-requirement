using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Glyph
{
    public string Description;
    public Sprite Icon;
}

public class FormGlyph : Glyph
{
}

public class EffectGlyph : Glyph
{
    public Color Color;
    public float Power;
    public Type Controller;
}

public class ModifierGlyph : Glyph
{
    public float Factor;
    public List<Enum> Compatibles; // if empty, compatible with everything
}