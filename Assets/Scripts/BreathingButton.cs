using UnityEngine;

public class BreathingButton : MonoBehaviour
{
    [Header("Breath Settings")]
    [SerializeField] private float maxScale = 1.08f;
    [SerializeField] private float minScale = 0.95f;
    [SerializeField] private float breathSpeed = 0.5f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * breathSpeed * 2f * Mathf.PI) + 1f) / 2f;

        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = originalScale * scale;
    }

    private void OnDisable()
    {
        transform.localScale = originalScale;
    }
}