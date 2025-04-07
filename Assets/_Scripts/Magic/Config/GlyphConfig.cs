using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using System.Reflection;

public class GlyphConfig
{
    private readonly Dictionary<Enum, Glyph> _glyphs;


    [Inject]
    public GlyphConfig(SpriteLoader iconLoader)
    {
        _glyphs = new(){

        // FORMS
        { Form.Self, new FormGlyph(){
            Description = "Applies the spell efect to the caster himself",
            Icon = iconLoader.GetIcon(Form.Self)
        } },
        { Form.Projectile, new FormGlyph(){
            Description = "Sends a magic projectile that applies the spell effect to the target (first thing it hits)",
            Icon = iconLoader.GetIcon(Form.Projectile)
        } },
        { Form.MagicSphere, new FormGlyph(){
            Description = "Creates a magic sphere that applies the spell effect to all things inside it",
            Icon = iconLoader.GetIcon(Form.MagicSphere)
        } },

        // EFFECTS
        { Effect.Launch, new EffectGlyph(){
            Description = "Sends the object high up in the air",
            Icon = iconLoader.GetIcon(Effect.Launch),
            Power = 10f,
            Color = Color.white
        }},
        { Effect.Enlarge, new EffectGlyph(){
            Description = "Makes object larger. There is a size limit",
            Icon = iconLoader.GetIcon(Effect.Enlarge),
            Power = 1.5f,
            Color = Color.red
        } },

        // MODIFIERS
        { Modifier.Amplify, new ModifierGlyph(){
            Description = "Increases the power of the effect",
            Icon = iconLoader.GetIcon(Modifier.Amplify),
            Factor = 1.5f,
            Compatibles = {}
        } },
        { Modifier.Accelerate, new ModifierGlyph(){
            Description = "Increases the speed of the spell projectile",
            Icon = iconLoader.GetIcon(Modifier.Accelerate),
            Factor = 1.5f,
            Compatibles = {Form.Projectile}
        }},
        { Modifier.Decelerate, new ModifierGlyph(){
            Description = "Decreases the speed of the spell projectile",
            Icon = iconLoader.GetIcon(Modifier.Decelerate),
            Factor = 1.5f,
            Compatibles = {Form.Projectile}
        }} };
    }

    public string GetDescription(Enum glyph) => GetValue<string>(glyph);
    public Sprite GetIcon(Enum glyph) => GetValue<Sprite>(glyph);
    public float GetPower(Effect effect) => GetValue<float>(effect);
    public Color GetColor(Effect effect) => GetValue<Color>(effect);
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
                return (T)field.GetValue(this);
            }
        }

        return default;
    }
}