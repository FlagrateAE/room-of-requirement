using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using System.Reflection;

public class GlyphConfig
{
    private readonly Dictionary<Enum, Glyph> _glyphs;
    private readonly SpriteLoader _iconLoader;

    [Inject]
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
            Controller = typeof(LaunchController)
        }},
        { Effect.Enlarge, new EffectGlyph(){
            Description = "Makes object larger. There is a size limit",
            Power = 1.5f,
            Color = Color.red,
            Controller = typeof(EnlargeController)
        } },

        // MODIFIERS
        { Modifier.Amplify, new ModifierGlyph(){
            Description = "Increases the power of the effect",
            Factor = 1.5f,
            Compatibles = {}
        } },
        { Modifier.Accelerate, new ModifierGlyph(){
            Description = "Increases the speed of the spell projectile",
            Factor = 1.5f,
            Compatibles = {Form.Projectile}
        }},
        { Modifier.Decelerate, new ModifierGlyph(){
            Description = "Decreases the speed of the spell projectile",
            Factor = 1.5f,
            Compatibles = {Form.Projectile}
        }} };
    }

    public string GetDescription(Enum glyph) => GetValue<string>(glyph);
    public Sprite GetIcon(Form form) => _iconLoader.GetIcon(form);
    public Sprite GetIcon(Effect effect) => _iconLoader.GetIcon(effect);
    public Sprite GetIcon(Modifier modifier) => _iconLoader.GetIcon(modifier);

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
                return (T)field.GetValue(this);
            }
        }

        return default;
    }
}