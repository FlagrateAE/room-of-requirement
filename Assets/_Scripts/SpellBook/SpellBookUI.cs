using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.Linq;

[RequireComponent(typeof(Canvas))]
public class SpellBookUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _iconPrefab;

    private Transform _formsContainer;
    private Transform _effectsContainer;
    private Transform _modifiersContainer;
    private Transform _spellContainer;

    private TextMeshProUGUI _nameInfo;
    private Image _iconInfo;
    private TextMeshProUGUI _descriptionInfo;
    private TextMeshProUGUI _incompatibleWarning;

    private readonly List<string> _currentSpell = new();

    private void Start()
    {
        _formsContainer = GetContainer("Forms");
        _effectsContainer = GetContainer("Effects");
        _modifiersContainer = GetContainer("Modifiers");

        Transform infoContainer = transform.Find("Info");
        _nameInfo = infoContainer.Find("Name").GetComponent<TextMeshProUGUI>();
        _iconInfo = transform.GetComponentInChildren<Image>();
        _descriptionInfo = infoContainer.Find("Description").GetComponent<TextMeshProUGUI>();

        _spellContainer = transform.Find("Spell");
        _incompatibleWarning = transform.Find("IncompatibleWarning").GetComponent<TextMeshProUGUI>();

        foreach (var formIcon in ConfigManager.Instance.GetIcons(GlyphType.Form))
        {
            AddIconToCatalogue(_formsContainer, formIcon.name);
        }
        foreach (var effectIcon in ConfigManager.Instance.GetIcons(GlyphType.Effect))
        {
            AddIconToCatalogue(_effectsContainer, effectIcon.name);
        }
        foreach (var modifierIcon in ConfigManager.Instance.GetIcons(GlyphType.Modifier))
        {
            AddIconToCatalogue(_modifiersContainer, modifierIcon.name);
        }

        DisplayGlyphInfo("Self");
    }

    private Transform GetContainer(string glyphType)
    {
        return transform.Find(glyphType).GetComponentInChildren<HorizontalLayoutGroup>().transform;
    }

    private void DisplayGlyphInfo(string glyphName)
    {
        _nameInfo.text = glyphName;
        _iconInfo.sprite = ConfigManager.Instance.GetIcon(glyphName);
        _descriptionInfo.text = ConfigManager.Instance.GetDescription(glyphName);
    }

    private void AddIconToCatalogue(Transform container, string glyphName)
    {
        GameObject icon = IconFactory.CatalogueIcon(glyphName, _iconPrefab, OnCatalogueIconClicked);
        icon.transform.SetParent(container);
    }

    private void AddGlyphToSpell(string glyphName)
    {
        GameObject icon = IconFactory.SpellIcon(glyphName, _iconPrefab, OnSpellIconClicked);
        icon.transform.SetParent(_spellContainer);

        foreach (string glyph in _currentSpell)
        {
            if (!ConfigManager.Instance.IsCompatible(glyph, glyphName))
            {
                _incompatibleWarning.text = $"{glyphName} is not compatible with {glyph}";
                _incompatibleWarning.enabled = true;
                return;
            }
        }

        _currentSpell.Add(glyphName);
    }

    private void RemoveGlyphFromSpell(GameObject glyphIcon)
    {
        Destroy(glyphIcon);
        _currentSpell.Remove(glyphIcon.name.Split("Icon")[0]);

        if (_incompatibleWarning.enabled)
            _incompatibleWarning.enabled = false;
    }

    private void OnCatalogueIconClicked(PointerEventData eventData)
    {
        string glyphName = eventData.pointerEnter.name.Split("Icon")[0];
        DisplayGlyphInfo(glyphName);

        if (eventData.button == PointerEventData.InputButton.Right)
            AddGlyphToSpell(glyphName);
    }

    private void OnSpellIconClicked(PointerEventData eventData)
    {
        string glyphName = eventData.pointerEnter.name.Split("Icon")[0];

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                DisplayGlyphInfo(glyphName);
                break;
            case PointerEventData.InputButton.Right:
                RemoveGlyphFromSpell(eventData.pointerEnter);
                break;
        }
    }

    private static class IconFactory
    {
        public static GameObject CatalogueIcon(string glyphName, GameObject iconPrefab, Action<PointerEventData> callback)
        {
            GameObject icon = CreateIcon(glyphName, iconPrefab);
            AttachEventListener(icon, IconMode.Catalogue, callback);

            return icon;
        }

        public static GameObject SpellIcon(string glyphName, GameObject iconPrefab, Action<PointerEventData> callback)
        {
            GameObject icon = CreateIcon(glyphName, iconPrefab);
            AttachEventListener(icon, IconMode.Spell, callback);

            return icon;
        }

        private static GameObject CreateIcon(string glyphName, GameObject iconPrefab)
        {
            GameObject icon = UnityEngine.Object.Instantiate(iconPrefab);
            icon.name = $"{glyphName}Icon";
            icon.GetComponent<Image>().sprite = ConfigManager.Instance.GetIcon(glyphName);

            return icon;
        }

        private static void AttachEventListener(GameObject icon, IconMode mode, Action<PointerEventData> callback)
        {
            EventTrigger.Entry entry = new()
            {
                eventID = EventTriggerType.PointerClick,
            };

            entry.callback.AddListener((data) => callback(data as PointerEventData));
            icon.GetComponent<EventTrigger>().triggers.Add(entry);
        }

        private enum IconMode
        {
            Catalogue,
            Spell
        }
    }
}