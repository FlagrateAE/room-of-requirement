using System.Collections.Generic;
using System;

public class ModifierGlyph : Glyph
{
    public ModifierType Type;
    public float Factor;
    public List<Enum> Compatibles;
}

public enum Modifier
{
    Accelerate,
    Amplify,
    Dampen,
    Decelerate,
    // Pierce
}

public enum ModifierType
{
    Factor,
    Behavior
}