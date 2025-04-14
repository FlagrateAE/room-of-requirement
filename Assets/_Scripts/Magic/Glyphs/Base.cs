using UnityEngine;
using System;

public abstract class Glyph
{
    public string Description;
    public Sprite Icon;

    public static Enum FromIcon(GameObject icon)
    {
        string name = icon.name.Split("Icon")[0];
        Type glyphType = Type.GetType(name.Split(".")[0]);
        string glyph = name.Split(".")[1];

        return (Enum)Enum.Parse(glyphType, glyph);
    }

    public static string Name(Enum glyph) => $"{glyph.GetType().Name}.{glyph}";
}

public enum GlyphType
{
    Form,
    Effect,
    Modifier
}
