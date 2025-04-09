using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpellBuilder
{
    public readonly List<Enum> Spell = new();
    private SpellBuildState _state = SpellBuildState.Form;

    // Functional glyphs are Forms and Effects
    public Enum LastFunctionalGlyph { get; private set; }

    public void Add(Enum glyph, out (bool, bool, bool) nextHighlights)
    {
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
        Debug.Log(nextHighlights);
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
        _ => (false, false, false),
    };

    private enum SpellBuildState
    {
        Form,
        FormModifiersOrEffect,
        EffectModifiers
    }
}