using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class SpellBookUI : MonoBehaviour
{
    private Canvas _canvas;
    private PlayerCamera _playerCamera;

    private HorizontalLayoutGroup _formsContainer;
    private HorizontalLayoutGroup _effectsContainer;
    private HorizontalLayoutGroup _modifiersContainer;

    private TextMeshProUGUI _nameInfo;
    private Image _iconInfo;
    private TextMeshProUGUI _descriptionInfo;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _playerCamera = GetComponentInParent<PlayerCamera>();

        _formsContainer = GetContainer("Forms");
        _effectsContainer = GetContainer("Effects");
        _modifiersContainer = GetContainer("Modifiers");

        Transform infoContainer = transform.Find("Info");
        _nameInfo = infoContainer.Find("Name").GetComponent<TextMeshProUGUI>();
        _iconInfo = transform.GetComponentInChildren<Image>();
        _descriptionInfo = infoContainer.Find("Description").GetComponent<TextMeshProUGUI>();

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