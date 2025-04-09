using UnityEngine;
using System;
using System.Collections.Generic;

public class SpellBuilder
{
    public readonly List<Enum> Spell = new();
    private SpellBuildState _state = SpellBuildState.Form;

    public void Add(Enum glyph, out (bool, bool, bool) nextHighlights)
    {
        Spell.Add(glyph);

        if (
            _state == SpellBuildState.FormModifiersOrEffect &&
            typeof(Effect).IsAssignableFrom(glyph.GetType())
        )
            _state = SpellBuildState.EffectModifiers;

        else _state++;

        nextHighlights = GetHighlights();
    }

    public void ClearSpell(out (bool, bool, bool) nextHighlights)
    {
        Spell.Clear();
        _state = SpellBuildState.Form;

        nextHighlights = GetHighlights();
    }

    public Enum LastGlyph => Spell.Count > 0 ? Spell[^1] : null;

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