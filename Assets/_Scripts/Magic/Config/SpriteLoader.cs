using System.IO;
using UnityEngine;

public class SpriteLoader
{
    private readonly Sprite[] _formIcons;
    private readonly Sprite[] _effectIcons;
    private readonly Sprite[] _modifierIcons;

    public SpriteLoader(string spriteFolder)
    {
        _formIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "FormIcons"));
        _effectIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "EffectIcons"));
        _modifierIcons = LoadSpriteSheet(Path.Combine(spriteFolder, "ModifierIcons"));
    }

    private Sprite[] LoadSpriteSheet(string spriteStyleSheetPath)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteStyleSheetPath);

        if (sprites.Length == 0)
            throw new System.Exception($"No sprites found in {spriteStyleSheetPath}");

        return sprites;
    }

    public Sprite GetIcon(Form form) => _formIcons[(int)form];
    public Sprite GetIcon(Effect effect) => _effectIcons[(int)effect];
    public Sprite GetIcon(Modifier modifier) => _modifierIcons[(int)modifier];
}