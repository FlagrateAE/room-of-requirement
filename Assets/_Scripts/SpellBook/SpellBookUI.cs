using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class SpellBookUI : MonoBehaviour
{
    private GlyphConfig _config;
    private IconManager _iconManager;

    [SerializeField]
    private GameObject _iconPrefab;

    private Transform _formsContainer;
    private Transform _effectsContainer;
    private Transform _modifiersContainer;
    private Transform _spellContainer;

    private TextMeshProUGUI _nameInfo;
    private Image _iconInfo;
    private TextMeshProUGUI _descriptionInfo;
    private TextMeshProUGUI _invalidSpellWarning;

    private readonly List<Enum> _currentSpellRaw = new();

    [Inject]
    public void Initialize(GlyphConfig config, IconManager iconManager)
    {
        _config = config;
        _iconManager = iconManager;
    }

    private void Start()
    {
        _nameInfo = transform.Find("Info").Find("Name").GetComponent<TextMeshProUGUI>();
        _iconInfo = transform.Find("Info").GetComponentInChildren<Image>();
        _descriptionInfo = transform.Find("Info").Find("Description").GetComponent<TextMeshProUGUI>();

        DisplayGlyphInfo(Form.Self);
    }

    private Transform GetContainer(string glyphType)
    {
        return transform.Find(glyphType).GetComponentInChildren<HorizontalLayoutGroup>().transform;
    }

    private void DisplayGlyphInfo(Enum glyph)
    {
        _nameInfo.text = glyph.ToString();
        _iconInfo.sprite = _config.GetIcon(glyph);
        _descriptionInfo.text = _config.GetDescription(glyph);
    }

    private void AddIconToCatalogue(Transform container, Enum glyph)
    {
        GameObject icon = IconFactory.CatalogueIcon(glyph, _iconPrefab, _config.GetIcon(glyph), OnCatalogueIconClicked);
        icon.transform.SetParent(container, false);
    }

    private void AddGlyphToSpell(Enum glyph)
    {
        GameObject icon = IconFactory.SpellIcon(glyph, _iconPrefab, _config.GetIcon(glyph), OnSpellIconClicked);
        icon.transform.SetParent(_spellContainer, false);
        _currentSpellRaw.Add(glyph);
    }

    private void RemoveGlyphFromSpell(GameObject glyphIcon)
    {
        _currentSpellRaw.Remove(Glyph.FromIcon(glyphIcon));
        Destroy(glyphIcon);
    }

    private void OnCatalogueIconClicked(PointerEventData eventData)
    {
        Enum glyph = Glyph.FromIcon(eventData.pointerEnter);
        DisplayGlyphInfo(glyph);

        if (eventData.button == PointerEventData.InputButton.Right)
            AddGlyphToSpell(glyph);
    }

    private void OnSpellIconClicked(PointerEventData eventData)
    {
        Enum glyph = Glyph.FromIcon(eventData.pointerEnter);

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                DisplayGlyphInfo(glyph);
                break;
            case PointerEventData.InputButton.Right:
                RemoveGlyphFromSpell(eventData.pointerEnter);
                break;
        }
    }

    private static class IconFactory
    {
        public static GameObject CatalogueIcon(Enum glyph, GameObject iconPrefab, Sprite sprite, Action<PointerEventData> callback)
        {
            GameObject icon = CreateIcon(Glyph.Name(glyph), iconPrefab, sprite);
            AttachEventListener(icon, IconMode.Catalogue, callback);

            return icon;
        }

        public static GameObject SpellIcon(Enum glyph, GameObject iconPrefab, Sprite sprite, Action<PointerEventData> callback)
        {
            GameObject icon = CreateIcon(Glyph.Name(glyph), iconPrefab, sprite);
            AttachEventListener(icon, IconMode.Spell, callback);

            return icon;
        }

        [Inject]
        private static GameObject CreateIcon(string glyphName, GameObject iconPrefab, Sprite sprite)
        {
            GameObject icon = Instantiate(iconPrefab);
            icon.name = $"{glyphName}Icon";
            icon.GetComponent<Image>().sprite = sprite;

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