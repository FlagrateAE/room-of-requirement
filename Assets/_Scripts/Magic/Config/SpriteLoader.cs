using System;
using System.IO;
using UnityEngine;
using Zenject;

public class SpriteLoader
{
    private readonly Sprite[] _formIcons;
    private readonly Sprite[] _effectIcons;
    private readonly Sprite[] _modifierIcons;

    public SpriteLoader(string spriteFolder)
    {
        _formIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "forms"));
        _effectIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "effects"));
        _modifierIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "modifiers"));
    }

    private Sprite[] LoadSpriteSheet(string spriteStyleSheetPath)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteStyleSheetPath);

        if (sprites.Length == 0)
            Debug.LogWarning($"No sprites found in {spriteStyleSheetPath}");

        return sprites;
    }

    public Sprite GetIcon(Enum glyph) => glyph switch
    {
        Form form => _formIcons[(int)form],
        Effect effect => _effectIcons[(int)effect],
        Modifier modifier => _modifierIcons[(int)modifier],
        _ => null,
    };

    public Sprite[] GetIcons(GlyphType glyphType) => glyphType switch
    {
        GlyphType.Form => _formIcons,
        GlyphType.Effect => _effectIcons,
        GlyphType.Modifier => _modifierIcons,
        _ => null,
    };
}