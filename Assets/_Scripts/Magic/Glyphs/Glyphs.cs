using UnityEngine;
using System;

public abstract class Glyph
{
    public readonly string Name;
    public Sprite Icon;
    public readonly string Description;
}

public class Form : Glyph
{

}

public class Effect : Glyph
{
    public Color Color;
    public readonly float Power;
    public readonly Type Controller;
}

public class Modifier : Glyph
{
    public readonly float Factor;
}