using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;
    public float shakeAmount = 0.05f;
    public int shakeCount = 4;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
        else
            Debug.LogError("HitEffect: No SpriteRenderer found on " + gameObject.name);
    }

    public void PlayHitEffect()
    {
        if (sr == null) return;
        StopAllCoroutines();
        StartCoroutine(HitEffectRoutine());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayHitEffect();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            sr.color = Color.green;
        }

    }


    private IEnumerator HitEffectRoutine()
    {
        // Flash
        sr.color = flashColor;

        // Shake (left-right jitter)
        Vector3 originalPos = transform.localPosition;

        for (int i = 0; i < shakeCount; i++)
        {
            transform.localPosition = originalPos + (Vector3.right * shakeAmount);
            yield return new WaitForSeconds(flashDuration / (shakeCount * 2));
            transform.localPosition = originalPos + (Vector3.left * shakeAmount);
            yield return new WaitForSeconds(flashDuration / (shakeCount * 2));
        }

        transform.localPosition = originalPos;
        sr.color = originalColor;
    }
}

