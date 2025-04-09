using System;
using System.Collections.Generic;

public class SpellBuilder
{
    public readonly List<Enum> Spell = new();
    private SpellBuildState _state = SpellBuildState.Form;

    // Functional glyphs are Forms and Effects
    public Enum LastFunctionalGlyph { get; private set; }

    public void TryAdd(Enum glyph, out (bool, bool, bool) nextHighlights)
    {
        if (Spell.Count == 8)
        {
            _state = SpellBuildState.MaxLengthReached;
            nextHighlights = GetHighlights();
            return;
        }

        Spell.Add(glyph);

        switch (glyph)
        {
            case Form:
                _state = SpellBuildState.FormModifiersOrEffect;
                LastFunctionalGlyph = glyph;
                break;
            case Effect:
                _state = SpellBuildState.EffectModifiers;
                LastFunctionalGlyph = glyph;
                break;
            default:
                break;
        }

        nextHighlights = GetHighlights();
    }

    public void ClearSpell(out (bool, bool, bool) nextHighlights)
    {
        Spell.Clear();
        _state = SpellBuildState.Form;

        nextHighlights = GetHighlights();
    }


    private (bool, bool, bool) GetHighlights() => _state switch
    {
        SpellBuildState.Form => (true, false, false),
        SpellBuildState.FormModifiersOrEffect => (false, true, true),
        SpellBuildState.EffectModifiers => (false, false, true),
        SpellBuildState.MaxLengthReached => (false, false, false),
        _ => (false, false, false),
    };

    private enum SpellBuildState
    {
        Form,
        FormModifiersOrEffect,
        EffectModifiers,
        MaxLengthReached
    }
}