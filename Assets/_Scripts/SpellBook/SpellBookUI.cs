using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class SpellBookUI : MonoBehaviour
{
    private HorizontalLayoutGroup _formsContainer;
    private HorizontalLayoutGroup _effectsContainer;
    private HorizontalLayoutGroup _modifiersContainer;

    private TextMeshPro _nameInfo;
    private Image _iconInfo;
    private TextMeshPro _descriptionInfo;

    private void Start()
    {
        _formsContainer = GetContainer("Forms");
        _effectsContainer = GetContainer("Effects");
        _modifiersContainer = GetContainer("Modifiers");

        _nameInfo = transform.Find("Info/Name").GetComponent<TextMeshPro>();
        _iconInfo = transform.Find("Info/Icon").GetComponent<Image>();
        _descriptionInfo = transform.Find("Info/Description").GetComponent<TextMeshPro>();

        foreach (var formIcon in ConfigManager.Instance.GetIcons(GlyphType.Form))
        {
            AddIconToContainer(_formsContainer, formIcon.name, formIcon);
        }
        foreach (var effectIcon in ConfigManager.Instance.GetIcons(GlyphType.Effect))
        {
            AddIconToContainer(_effectsContainer, effectIcon.name, effectIcon);
        }
        foreach (var modifierIcon in ConfigManager.Instance.GetIcons(GlyphType.Modifier))
        {
            AddIconToContainer(_modifiersContainer, modifierIcon.name, modifierIcon);
        }

    }

    private HorizontalLayoutGroup GetContainer(string glyphType)
    {
        return transform.Find(glyphType).GetComponentInChildren<HorizontalLayoutGroup>();
    }

    private void AddIconToContainer(HorizontalLayoutGroup container, string glyphName, Sprite iconSprite)
    {
        int IconSize = 60;

        GameObject icon = new(name: $"{glyphName}Icon", typeof(Image), typeof(Button));

        icon.GetComponent<RectTransform>().sizeDelta = Vector2.one * IconSize;
        icon.GetComponent<Image>().sprite = iconSprite;
        icon.GetComponent<Image>().preserveAspect = true;
        icon.GetComponent<Button>().onClick.AddListener(() => DisplayInfo(
            glyphName, iconSprite,
            ConfigManager.Instance.GetDescription(glyphName)
        ));
        
        icon.transform.SetParent(container.transform);
    }

    private void DisplayInfo(string glyphName, Sprite icon, string glyphDescription)
    {
        _nameInfo.text = glyphName;
        _iconInfo.sprite = icon;
        _descriptionInfo.text = glyphDescription;
    }
}