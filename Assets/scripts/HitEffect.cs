using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector3 originalPosition;

    [Header("Settings")]
    public float flashDuration = 0.05f;
    public float shakeDistance = 0.1f;
    public int shakeCount = 5;
    public Color flashColor = Color.red;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalPosition = transform.localPosition;
    }

    public void PlayHitEffect()
    {
        StartCoroutine(FlashAndShake());
    }

    private System.Collections.IEnumerator FlashAndShake()
    {
        if (sr != null)
        {
            sr.color = flashColor;
        }

        for (int i = 0; i < shakeCount; i++)
        {
            float offsetX = (i % 2 == 0 ? 1 : -1) * shakeDistance;
            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(flashDuration);
        }

        transform.localPosition = originalPosition;

        if (sr != null)
        {
            sr.color = Color.white;
        }
    }
}
