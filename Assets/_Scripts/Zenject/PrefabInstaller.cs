using UnityEngine;
using Zenject;

public class PrefabInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _projectilePrefab;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().WithId("Projectile").FromInstance(_projectilePrefab).AsSingle();
    }
}