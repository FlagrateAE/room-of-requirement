using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField]
    private Canvas _spellBookCanvas;
    [SerializeField]
    private GameObject _iconPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(_spellBookCanvas);
        Container.BindInstance(_iconPrefab);
    }
}