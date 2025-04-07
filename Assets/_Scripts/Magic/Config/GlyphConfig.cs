using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class GlyphConfig
{
    private readonly Dictionary<Form, FormGlyph> _forms;
    private readonly Dictionary<Effect, EffectGlyph> _effects;
    private readonly Dictionary<Modifier, ModifierGlyph> _modifiers;

    [Inject]
    public GlyphConfig(SpriteLoader iconLoader)
    {
        _forms = new(){
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
        } } };

        _effects = new(){
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
        } } };

        _modifiers = new(){
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
}