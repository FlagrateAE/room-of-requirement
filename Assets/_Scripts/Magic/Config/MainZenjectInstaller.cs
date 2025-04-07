using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    private string _iconsPath = "Glyphs";

    public override void InstallBindings()
    {
        Container.Bind<SpriteLoader>()
        .FromMethod(_ => new SpriteLoader(_iconsPath)).AsSingle().NonLazy();
    }
}