using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SpellBookUI : MonoBehaviour
{
    private FormConfig _formConfig;
    private EffectConfig _effectConfig;
    private ModifierConfig _modifierConfig;

    private HorizontalLayoutGroup _formsContainer;
    private HorizontalLayoutGroup _effectsContainer;
    private HorizontalLayoutGroup _modifiersContainer;

    private void Start()
    {
        _formsContainer = GetContainer("Forms");
        _effectsContainer = GetContainer("Effects");
        _modifiersContainer = GetContainer("Modifiers");

        foreach (var formIcon in _formConfig.GetAllIcons())
        {
            AddIconToContainer(_formsContainer, formIcon.name, formIcon);
        }
        foreach (var effectIcon in _effectConfig.GetAllIcons())
        {
            AddIconToContainer(_effectsContainer, effectIcon.name, effectIcon);
        }
        foreach (var modifierIcon in _modifierConfig.GetAllIcons())
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

        GameObject icon = new(name: $"{glyphName}Icon", typeof(Image));

        icon.GetComponent<Image>().sprite = iconSprite;
        icon.GetComponent<Image>().preserveAspect = true;
        icon.GetComponent<RectTransform>().sizeDelta = Vector2.one * IconSize;

        icon.transform.SetParent(container.transform);
    }

    public void TransferConfigs(FormConfig formConfig, EffectConfig effectConfig, ModifierConfig modifierConfig)
    {
        _formConfig = formConfig;
        _effectConfig = effectConfig;
        _modifierConfig = modifierConfig;
    }
}