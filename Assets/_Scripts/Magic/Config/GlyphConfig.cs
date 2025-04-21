using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public class GlyphConfig
{
    private readonly Dictionary<Enum, Glyph> _glyphs;
    private readonly SpriteLoader _iconLoader;
    private readonly int _cacheSize;
    private readonly CacheEvictionPolicy _evictionPolicy;

    public GlyphConfig(SpriteLoader iconLoader, int cacheSize, CacheEvictionPolicy evictionPolicy)
    {
        _iconLoader = iconLoader;
        _cacheSize = cacheSize;
        _evictionPolicy = evictionPolicy;

        _glyphs = new(){

        // FORMS
        { Form.MagicSphere, new FormGlyph(){
            Description = "Creates a magic sphere that applies the spell effect to all things inside it",
        } },
        { Form.Projectile, new FormGlyph(){
            Description = "Sends a magic projectile that applies the spell effect to the target (first thing it hits)",
        } },
        { Form.Self, new FormGlyph(){
            Description = "Applies the spell efect to the caster himself",
        } },

        // EFFECTS
        { Effect.Enlarge, new EffectGlyph(){
            Description = "Makes object larger. There is a size limit",
            Power = 1.25f,
            Color = Color.red,
        } },
        { Effect.Launch, new EffectGlyph(){
            Description = "Sends the object high up in the air",
            Power = 10f,
            Color = Color.white,
        }},

        // MODIFIERS
        { Modifier.Accelerate, new ModifierGlyph(){
            Description = "Increases the speed of the spell projectile",
            Compatibles = new(){Form.Projectile},
            Factor = 1.25f
        }},
        { Modifier.Amplify, new ModifierGlyph(){
            Description = "Increases the power of the effect",
            Compatibles = new(){Effect.Launch, Effect.Enlarge},
            Factor = 1.25f
        } },
        { Modifier.Dampen, new ModifierGlyph(){
            Description = "Decreases the power of the effect",
            Compatibles = new(){Effect.Launch, Effect.Enlarge},
            Factor = 0.75f
        } },
        { Modifier.Decelerate, new ModifierGlyph(){
            Description = "Decreases the speed of the spell projectile",
            Compatibles = new(){Form.Projectile},
            Factor = 0.75f
        }},
        { Modifier.Pierce, new ModifierGlyph(){
            Description = "Allows the spell projectile to pass through objects instead of destroying on 1st hit. 1 Pierce = 1 object",
            Compatibles = new(){Form.Projectile},
        }},
        };
    }

    public Form[] GetAllForms() => _glyphs.Keys.OfType<Form>().ToArray();
    public Effect[] GetAllEffects() => _glyphs.Keys.OfType<Effect>().ToArray();
    public Modifier[] GetAllModifiers() => _glyphs.Keys.OfType<Modifier>().ToArray();

    public string GetDescription(Enum glyph) => GetValue<string>(glyph);
    public Sprite GetIcon(Enum glyph) => _iconLoader.GetIcon(glyph);

    public Type GetFormCaster(Form form) => Type.GetType($"{form}Caster");

    public float GetPower(Effect effect) => GetValue<float>(effect);
    public Color GetColor(Effect effect) => GetValue<Color>(effect);
    public Type GetEffectController(Effect effect) => Type.GetType($"{effect}Controller");

    public float GetFactor(Modifier modifier) => GetValue<float>(modifier);
    public ModifierType GetModifierType(Modifier modifier) => GetValue<ModifierType>(modifier);
    public List<Enum> GetCompatibles(Modifier modifier) => GetValue<List<Enum>>(modifier);

    private readonly Dictionary<(Enum, Type), object> _valueCache = new();
    private T GetValue<T>(Enum glyph)
    {
        var key = (glyph, typeof(T));
        if (_valueCache.TryGetValue(key, out var cachedValue))
        {
            Debug.Log($"Retrieved {typeof(T).Name} from cache for {glyph}");
            _usageOrder.Remove(key);
            _usageOrder.AddLast(key);
            return (T)cachedValue;
        }
        else
        {
            if (_valueCache.Count >= _cacheSize)
            {
                EvictCache();
            }

            Glyph found = _glyphs[glyph];
            if (TryReflectFromGlyph(found, out T value))
            {
                _valueCache[key] = value;
                _usageOrder.AddLast(key);
                Debug.Log($"Cached {typeof(T).Name} from {glyph}, cache size: {_valueCache.Count}");
                return value;
            }
        }

        return default;
    }

    private bool TryReflectFromGlyph<T>(Glyph glyph, out T value)
    {
        FieldInfo[] fields = glyph.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (typeof(T).IsAssignableFrom(field.FieldType))
            {
                value = (T)field.GetValue(glyph);
                return true;
            }
        }

        value = default;
        return false;
    }

    private readonly LinkedList<(Enum glyph, Type type)> _usageOrder = new();
    private void EvictCache()
    {
        if (_evictionPolicy == CacheEvictionPolicy.LRU && _usageOrder.Count > 0)
        {
            var lruKey = _usageOrder.First.Value;
            _usageOrder.RemoveFirst();
            _valueCache.Remove(lruKey);

            Debug.Log($"Evicted {lruKey.glyph} ({lruKey.type.Name}) from cache, cache size: {_valueCache.Count}");
        }
    }
}

public enum CacheEvictionPolicy
{
    LRU
}