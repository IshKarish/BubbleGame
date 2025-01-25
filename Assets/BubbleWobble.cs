using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleWobble : MonoBehaviour
{
    [Header("Wobble Settings")]
    public float wobbleSpeedMin = 1; // Speed of the wobble
    public float wobbleSpeedMax = 3;
    
    [Space]
    
    public float wobbleIntensityMin = 0.1f; // Intensity of the 
    public float wobbleIntensityMax = 0.5f;

    private Vector3 _originalScale;
    private float _wobbleTime;

    private float _wobbleSpeed;
    private float _wobbleIntensity;

    private void Awake()
    {
        _wobbleSpeed = Random.Range(wobbleSpeedMin, wobbleSpeedMax + 1);
        _wobbleIntensity = Random.Range(wobbleIntensityMin, wobbleIntensityMax + 1);
    }

    void Start()
    {
        // Store the original scale of the sprite
        _originalScale = transform.localScale;
    }

    void Update()
    {
        // Increment time to animate the wobble
        _wobbleTime += Time.deltaTime * _wobbleSpeed;

        // Calculate scale modifications
        float wobbleX = Mathf.Sin(_wobbleTime) * _wobbleIntensity;
        float wobbleY = Mathf.Cos(_wobbleTime) * _wobbleIntensity;

        // Apply the wobble effect to the sprite's scale
        transform.localScale = new Vector3(
            _originalScale.x + wobbleX,
            _originalScale.y + wobbleY,
            _originalScale.z
        );
    }
}