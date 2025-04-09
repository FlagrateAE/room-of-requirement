using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

[Serializable]
public class IconManager
{
    private readonly GlyphConfig _config;
    private readonly Canvas _spellBookCanvas;
    private readonly GameObject _iconPrefab;

    public event Action<Enum> OnIconDisplay;

    private readonly Transform _formsContainer;
    private readonly Transform _effectsContainer;
    private readonly Transform _modifiersContainer;
    private readonly Transform _spellContainer;

    private readonly Dictionary<Enum, Image> _forms = new();
    private readonly Dictionary<Enum, Image> _effects = new();
    private readonly Dictionary<Enum, Image> _modifiers = new();

    [SerializeField]
    private SpellBuildState _state = SpellBuildState.Form;
    [SerializeField]
    private List<Enum> _currentSpell = new();

    public IconManager(GlyphConfig config, Canvas spellBookCanvas, GameObject iconPrefab)
    {
        _config = config;
        _spellBookCanvas = spellBookCanvas;
        _iconPrefab = iconPrefab;

        _formsContainer = GetContainer(GlyphType.Form);
        foreach (var form in _config.GetAllForms())
            AddCatalogueIcon(form);

        _effectsContainer = GetContainer(GlyphType.Effect);
        foreach (var effect in _config.GetAllEffects())
            AddCatalogueIcon(effect);

        _modifiersContainer = GetContainer(GlyphType.Modifier);
        foreach (var modifier in _config.GetAllModifiers())
            AddCatalogueIcon(modifier);

        _spellContainer = _spellBookCanvas.transform.Find("Spell");
        HighlightGroupsByState();
    }

    private void AddCatalogueIcon(Enum glyph)
    {
        GameObject icon = UnityEngine.Object.Instantiate(_iconPrefab);
        icon.name = $"{Glyph.Name(glyph)}Icon";
        icon.GetComponent<Image>().sprite = _config.GetIcon(glyph);
        AttachEventListener(icon, IconType.Catalogue);

        switch (glyph)
        {
            case Form:
                AddIconToContainer(_formsContainer, icon);
                RegisterIcon(glyph, icon, _forms);
                break;
            case Effect:
                AddIconToContainer(_effectsContainer, icon);
                RegisterIcon(glyph, icon, _effects);
                break;
            case Modifier:
                AddIconToContainer(_modifiersContainer, icon);
                RegisterIcon(glyph, icon, _modifiers);
                break;
        }

        static void AddIconToContainer(Transform container, GameObject icon)
        {
            icon.transform.SetParent(container, false);
        }

        static void RegisterIcon(Enum glyph, GameObject icon, Dictionary<Enum, Image> targetIcons)
        {
            targetIcons.Add(glyph, icon.GetComponent<Image>());
        }
    }

    private void AddGlyphToSpell(GameObject catalogueIcon)
    {
        if (!IsActive(catalogueIcon)) return;

        GameObject spellIcon = UnityEngine.Object.Instantiate(
            catalogueIcon, _spellContainer, worldPositionStays: false
        );
        spellIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
        AttachEventListener(spellIcon, IconType.Spell);

        Enum glyph = Glyph.FromIcon(catalogueIcon);
        _currentSpell.Add(glyph);
        NextState(glyph);

        static bool IsActive(GameObject icon) => icon.GetComponent<Image>().color.a == 1f;
    }

    private Transform GetContainer(GlyphType glyphType)
    {
        return _spellBookCanvas.transform
        .Find($"{glyphType}s")
        .GetComponentInChildren<HorizontalLayoutGroup>().transform;
    }

    private void NextState(Enum addedGlyph)
    {
        if (_state == SpellBuildState.Form && typeof(Effect).IsAssignableFrom(addedGlyph.GetType()))
            _state = SpellBuildState.EffectModifiers;
        else
            _state++;

        HighlightGroupsByState();
    }

    private void HighlightGroupsByState()
    {
        Debug.Log(_state);

        switch (_state)
        {
            case SpellBuildState.Form:
                HighlightGlyphs(true, false, false);
                break;
            case SpellBuildState.FormModifiersOrEffect:
                HighlightGlyphs(false, true, true);
                break;
            case SpellBuildState.EffectModifiers:
                HighlightGlyphs(false, false, true);
                break;
        }
    }

    private void HighlightGlyphs(bool forms, bool effects, bool modifiers)
    {
        HighlightIcons(_forms, forms);
        HighlightIcons(_effects, effects);

        if (modifiers)
            HighlightIcons(GetCompatibleModifiers(_currentSpell[^1]), true);
        else
            HighlightIcons(_modifiers, false);
    }

    private void HighlightIcons(Dictionary<Enum, Image> icons, bool visible)
    {
        foreach (var icon in icons)
        {
            var color = icon.Value.color;
            color.a = visible ? 1 : 0.5f;
            icon.Value.color = color;
        }
    }

    private Dictionary<Enum, Image> GetCompatibleModifiers(Enum targetGlyph)
    {
        return _modifiers.Where(
            kvp =>
            {
                Modifier modifier = (Modifier)kvp.Key;
                return _config.GetCompatibles(modifier).Contains(targetGlyph);
            }
        ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private void AttachEventListener(GameObject icon, IconType type)
    {
        EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerClick };

        Action<PointerEventData> callback = type switch
        {
            IconType.Catalogue => OnCatalogueIconClicked,
            IconType.Spell => OnSpellIconClicked,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

        entry.callback.AddListener((data) => callback(data as PointerEventData));
        icon.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    private void OnCatalogueIconClicked(PointerEventData eventData)
    {
        Enum glyph = Glyph.FromIcon(eventData.pointerEnter);
        OnIconDisplay?.Invoke(glyph);

        if (eventData.button == PointerEventData.InputButton.Right)
            AddGlyphToSpell(eventData.pointerEnter);
    }

    private void OnSpellIconClicked(PointerEventData eventData)
    {
        Enum glyph = Glyph.FromIcon(eventData.pointerEnter);
        OnIconDisplay?.Invoke(glyph);
    }

    private enum SpellBuildState
    {
        Form,
        FormModifiersOrEffect,
        EffectModifiers,
    }

    private enum IconType
    {
        Catalogue,
        Spell
    }
}