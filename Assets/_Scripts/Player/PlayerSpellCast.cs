using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpellCast : MonoBehaviour
{
    private Transform _playerCamera;
    private InputAction _castInput;

    [SerializeField]
    private GameObject _spellPrefab;
    [SerializeField]
    public SpellData CurrentSpell;

    private void Start()
    {
        _castInput = InputSystem.actions.FindAction("Attack");
        _playerCamera = transform.Find("Main Camera");
    }

    private void Update()
    {
        if (_castInput.WasPressedThisFrame())
        {
            Cast(CurrentSpell);
        }
    }

    public void ArmSpell(SpellData spellData) => gameObject.AddComponent(spellData.FormCaster);

    private void Cast(SpellData spellData)
    {
        if (spellData == null) return;

        switch (spellData.Form)
        {
            case Form.Self:
                if (
                    TryGetComponent<SpellInteractable>(out var interactable) &&
                    interactable.IsCompatibleWith(spellData.Effect)
                )
                    interactable.ApplySpell(spellData);

                break;

            case Form.Projectile:
                GameObject spellInstance = Instantiate(_spellPrefab, _playerCamera.position, _playerCamera.rotation);

                SpellProjectile spellComponent = spellInstance.GetComponent<SpellProjectile>();
                spellComponent.LoadSpellData(spellData);

                break;
        }
    }
}