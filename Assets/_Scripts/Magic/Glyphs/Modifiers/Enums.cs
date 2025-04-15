using System.Collections.Generic;
using System;

public class ModifierGlyph : Glyph
{
    public List<Enum> Compatibles;
    public Action<SpellData> Modify;
}

public enum Modifier
{
    Amplify,
    Accelerate,
    Decelerate
}