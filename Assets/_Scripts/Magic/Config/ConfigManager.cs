using UnityEngine;
using System;
using System.Collections.Generic;

public enum GlyphType { Form, Effect, Modifier }

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [SerializeField]
    private FormConfig _formConfig;
    [SerializeField]
    private EffectConfig _effectConfig;
    [SerializeField]
    private ModifierConfig _modifierConfig;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public Sprite GetIcon(string glyphName) => GetConfig(glyphName).GetIcon(glyphName);
    public string GetDescription(string glyphName) => GetConfig(glyphName).GetDescription(glyphName);

    public float GetEffectPower(string effectName) => _effectConfig.GetPower(effectName);
    public Color GetEffectColor(string effectName) => _effectConfig.GetColor(effectName);

    public float GetModifierFactor(string modifierName) => _modifierConfig.GetFactor(modifierName);

    public List<Sprite> GetIcons(GlyphType type){
        return type switch
        {
            GlyphType.Form => _formConfig.GetAllIcons(),
            GlyphType.Effect => _effectConfig.GetAllIcons(),
            GlyphType.Modifier => _modifierConfig.GetAllIcons(),
            _ => null,
        };
    }

    public bool IsCompatible(string modifierName, string glyphName) => _modifierConfig.IsCompatible(modifierName, glyphName);

    public GlyphType GetGlyphType(string glyphName){
        if (_formConfig.GlyphExists(glyphName)) return GlyphType.Form;
        if (_effectConfig.GlyphExists(glyphName)) return GlyphType.Effect;
        if (_modifierConfig.GlyphExists(glyphName)) return GlyphType.Modifier;

        throw new Exception($"Glyph {glyphName} does not exist.");
    }

    private BaseConfig GetConfig(string glyphName)
    {
        if (_formConfig.GlyphExists(glyphName)) return _formConfig;
        if (_effectConfig.GlyphExists(glyphName)) return _effectConfig;
        if (_modifierConfig.GlyphExists(glyphName)) return _modifierConfig;

        throw new Exception($"Glyph {glyphName} does not exist in any config.");
    }
}