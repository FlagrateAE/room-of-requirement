using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    private string _iconsFolder = "Glyphs";

    [Header("Glyph Config caching params")]
    [SerializeField]
    private int _cacheSize = 8;
    [SerializeField]
    private CacheEvictionPolicy _evictionPolicy = CacheEvictionPolicy.LRU;

    public override void InstallBindings()
    {
        Container.Bind<SpriteLoader>().AsSingle().WithArguments(_iconsFolder).NonLazy();
        Container.Bind<GlyphConfig>().AsSingle().WithArguments(_cacheSize, _evictionPolicy).NonLazy();
    }
}