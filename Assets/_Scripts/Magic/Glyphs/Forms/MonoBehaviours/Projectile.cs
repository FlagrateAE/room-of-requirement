using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpellProjectile : MonoBehaviour
{
    public SpellData SpellData { get; private set; }

    private float _speed;
    private Rigidbody _rb;
    private bool _armed;

    // MODIFIERS DATA
    private int _pierceDepth = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _speed;
    }

    // Arm only when spell exits the player (to avoid player shooting themselves)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _armed = true;
    }

    private void OnTriggerEnter(Collider target)
    {
        if (_armed)
        {
            if (_pierceDepth == 0)
                Destroy(gameObject);
            else
            {
                _pierceDepth--;
            }

            if (
                target.gameObject.TryGetComponent(out SpellInteractable interactable) &&
                interactable.IsCompatibleWith(SpellData.Effect)
            )
            {
                interactable.ApplySpell(SpellData);
            }
        }


    }

    public void LoadSpellData(SpellData data)
    {
        SpellData = data;
        _speed = data.FlightSpeed;
        SetColor(data.Color);

        _pierceDepth = data.PierceDepth;
    }

    private void SetColor(Color color)
    {
        const float BrightenSaturation = -0.1f;
        Color.RGBToHSV(color, out float h, out float s, out float v);

        var spellParticles = GetComponent<ParticleSystem>().main;
        spellParticles.startColor = Color.HSVToRGB(h, Mathf.Clamp(s + BrightenSaturation, 0, 1), v);
    }
}
