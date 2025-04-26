using UnityEngine;

public interface IFormPoolable
{
    void OnGetFromPool(Transform spellSpawner);
    void OnReleaseToPool();
}