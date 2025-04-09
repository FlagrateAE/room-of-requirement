using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class SpellBook : MonoBehaviour
{
    private GlyphConfig _config;
    private IconManager _iconManager;
    private readonly SpellBuilder _spellBuilder = new();

    [SerializeField]
    private GameObject _iconPrefab;

    private TextMeshProUGUI _nameInfo;
    private Image _iconInfo;
    private TextMeshProUGUI _descriptionInfo;


    [Inject]
    public void Initialize(GlyphConfig config, IconManager iconManager)
    {
        _config = config;
        _iconManager = iconManager;

        _iconManager.OnIconDisplay += DisplayGlyphInfo;
        _iconManager.OnIconAddToSpell += AddGlyphToSpell;
    }

    private void Start()
    {
        _nameInfo = transform.Find("Info").Find("Name").GetComponent<TextMeshProUGUI>();
        _iconInfo = transform.Find("Info").GetComponentInChildren<Image>();
        _descriptionInfo = transform.Find("Info").Find("Description").GetComponent<TextMeshProUGUI>();

        ClearSpell();
        DisplayGlyphInfo(Form.Self);
    }

    private void DisplayGlyphInfo(Enum glyph)
    {
        _nameInfo.text = glyph.ToString();
        _iconInfo.sprite = _config.GetIcon(glyph);
        _descriptionInfo.text = _config.GetDescription(glyph);
    }

    private void AddGlyphToSpell(GameObject catalogueIcon)
    {
        if (_iconManager.TryAddToSpell(catalogueIcon))
        {
            _spellBuilder.Add(Glyph.FromIcon(catalogueIcon), out var nextHighlights);
            _iconManager.HighlightGlyphGroups(nextHighlights, _spellBuilder.LastGlyph);
        }
    }

    public void ClearSpell()
    {
        _spellBuilder.ClearSpell(out var nextHighlights);

        _iconManager.HighlightGlyphGroups(nextHighlights);
        _iconManager.ClearSpell();
    }
}