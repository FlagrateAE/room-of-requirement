using System.Collections.Generic;
using System;

public class ModifierGlyph : Glyph
{
    public float Factor;
    public List<Enum> Compatibles;
}

public enum Modifier
{
    Amplify,
    Accelerate,
    Decelerate
}