using System;
using System.Collections.Generic;

public class SpellBuilder
{
    private GlyphConfig _config;
    public SpellBuilder(GlyphConfig config) => _config = config;

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

    public SpellData BuildSpell()
    {
        if (!TryReadSpell(out var form, out var effect, out var modifiers))
            return null;

        SpellData result = new(
            form,
            effect,
            _config.GetController(effect),
            _config.GetColor(effect),
            _config.GetPower(effect)
        );

        if (modifiers != null)
            result.RegisterModifiers(modifiers, _config);

        return result;
    }

    private bool TryReadSpell(out Form form, out Effect effect, out List<Modifier> modifiers)
    {
        form = default;
        effect = default;
        modifiers = new();

        bool formPresent = false, effectPresent = false;

        foreach (var glyph in Spell)
        {
            switch (glyph)
            {
                case Form f:
                    form = f;
                    formPresent = true;
                    break;
                case Effect e:
                    effect = e;
                    effectPresent = true;
                    break;
                case Modifier m:
                    modifiers.Add(m);
                    break;
            }
        }

        return formPresent && effectPresent;
    }
}