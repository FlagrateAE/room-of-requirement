using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    private string _iconsFolder = "Glyphs";

    public override void InstallBindings()
    {
        Container.Bind<SpriteLoader>().FromMethod(_ => new SpriteLoader(_iconsFolder)).AsSingle().NonLazy();
        Container.Bind<GlyphConfig>().AsSingle().NonLazy();
        Container.Bind<IconManager>().AsSingle().NonLazy();
    }
}