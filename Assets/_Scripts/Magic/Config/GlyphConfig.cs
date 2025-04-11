using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public class GlyphConfig
{
    private readonly Dictionary<Enum, Glyph> _glyphs;
    private readonly SpriteLoader _iconLoader;

    public GlyphConfig(SpriteLoader iconLoader)
    {
        _iconLoader = iconLoader;
        _glyphs = new(){

        // FORMS
        { Form.Self, new FormGlyph(){
            Description = "Applies the spell efect to the caster himself",
        } },
        { Form.Projectile, new FormGlyph(){
            Description = "Sends a magic projectile that applies the spell effect to the target (first thing it hits)",
        } },
        { Form.MagicSphere, new FormGlyph(){
            Description = "Creates a magic sphere that applies the spell effect to all things inside it",
        } },

        // EFFECTS
        { Effect.Launch, new EffectGlyph(){
            Description = "Sends the object high up in the air",
            Power = 10f,
            Color = Color.white,
        }},
        { Effect.Enlarge, new EffectGlyph(){
            Description = "Makes object larger. There is a size limit",
            Power = 1.5f,
            Color = Color.red,
        } },

        // MODIFIERS
        { Modifier.Amplify, new ModifierGlyph(){
            Description = "Increases the power of the effect",
            Factor = 1.5f,
            Compatibles = new(){Effect.Launch, Effect.Enlarge}
        } },
        { Modifier.Accelerate, new ModifierGlyph(){
            Description = "Increases the speed of the spell projectile",
            Factor = 1.5f,
            Compatibles = new(){Form.Projectile}
        }},
        { Modifier.Decelerate, new ModifierGlyph(){
            Description = "Decreases the speed of the spell projectile",
            Factor = 0.5f,
            Compatibles = new(){Form.Projectile}
        }}
        };
    }

    public Form[] GetAllForms() => _glyphs.Keys.OfType<Form>().ToArray();
    public Effect[] GetAllEffects() => _glyphs.Keys.OfType<Effect>().ToArray();
    public Modifier[] GetAllModifiers() => _glyphs.Keys.OfType<Modifier>().ToArray();

    public string GetDescription(Enum glyph) => GetValue<string>(glyph);
    public Sprite GetIcon(Enum glyph) => _iconLoader.GetIcon(glyph);

    public float GetPower(Effect effect) => GetValue<float>(effect);
    public Color GetColor(Effect effect) => GetValue<Color>(effect);
    public Type GetController(Effect effect) => Type.GetType($"{effect}Controller");

    public float GetFactor(Modifier modifier) => GetValue<float>(modifier);
    public List<Enum> GetCompatibles(Modifier modifier) => GetValue<List<Enum>>(modifier);

    private T GetValue<T>(Enum glyph)
    {
        Glyph found = _glyphs[glyph];

        FieldInfo[] fields = found.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (typeof(T).IsAssignableFrom(field.FieldType))
            {
                return (T)field.GetValue(found);
            }
        }

        return default;
    }
}