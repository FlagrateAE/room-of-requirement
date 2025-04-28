using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightTwinkle : MonoBehaviour
{
    private Light _light;

    public float baseIntensity = 1.5f;
    public float intensityVariation = 0.5f;
    public float flickerSpeed = 2.0f;

    private float _noiseSeed;

    void Start()
    {
        _light = GetComponent<Light>();
        _noiseSeed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(_noiseSeed, Time.time * flickerSpeed);
        float flicker = (noise - 0.5f) * 2f * intensityVariation;
        _light.intensity = baseIntensity + flicker;
    }
}
