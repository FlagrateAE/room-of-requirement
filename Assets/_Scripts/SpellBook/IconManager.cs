using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IconManager
{
    private readonly GlyphConfig _config;
    private readonly Canvas _spellBookCanvas;
    private readonly GameObject _iconPrefab;

    private readonly Transform _formsContainer;
    private readonly Transform _effectsContainer;
    private readonly Transform _modifiersContainer;

    private readonly Dictionary<Enum, Image> _forms = new();
    private readonly Dictionary<Enum, Image> _effects = new();
    private readonly Dictionary<Enum, Image> _modifiers = new();

    public IconManager(GlyphConfig config, Canvas spellBookCanvas, GameObject iconPrefab)
    {
        _config = config;
        _spellBookCanvas = spellBookCanvas;
        _iconPrefab = iconPrefab;

        _formsContainer = GetContainer(GlyphType.Form);
        foreach (Enum form in Enum.GetValues(typeof(Form)))
            AddIcon(form);

        _effectsContainer = GetContainer(GlyphType.Effect);
        foreach (Enum effect in Enum.GetValues(typeof(Effect)))
            AddIcon(effect);

        _modifiersContainer = GetContainer(GlyphType.Modifier);
        foreach (Enum modifier in Enum.GetValues(typeof(Modifier)))
            AddIcon(modifier);
    }

    private void AddIcon(Enum glyph)
    {
        GameObject icon = UnityEngine.Object.Instantiate(_iconPrefab);
        icon.name = $"{Glyph.Name(glyph)}Icon";
        icon.GetComponent<Image>().sprite = _config.GetIcon(glyph);

        switch (glyph)
        {
            case Form _:
                AddIconToContainer(_formsContainer, icon);
                RegisterIcon(glyph, icon, _forms);
                break;
            case Effect _:
                AddIconToContainer(_effectsContainer, icon);
                RegisterIcon(glyph, icon, _effects);
                break;
            case Modifier _:
                AddIconToContainer(_modifiersContainer, icon);
                RegisterIcon(glyph, icon, _modifiers);
                break;
        }
    }

    private void AddIconToContainer(Transform container, GameObject icon)
    {
        icon.transform.SetParent(container, false);
    }

    private void RegisterIcon(Enum glyph, GameObject icon, Dictionary<Enum, Image> targetIcons)
    {
        targetIcons.Add(glyph, icon.GetComponent<Image>());
    }

    private Transform GetContainer(GlyphType glyphType)
    {
        return (Transform)_spellBookCanvas.transform
        .Find($"{glyphType}s")
        .GetComponentInChildren<HorizontalLayoutGroup>().transform;
    }

    public void SetGlyphGroupVisibility(bool forms, bool effects, bool modifiers)
    {
        SetIconGroupVisibility(_forms, forms);
        SetIconGroupVisibility(_effects, effects);
        SetIconGroupVisibility(_modifiers, modifiers);
    }

    private void SetIconGroupVisibility(Dictionary<Enum, Image> icons, bool visible)
    {
        foreach (var icon in icons)
        {

        }
    }
}