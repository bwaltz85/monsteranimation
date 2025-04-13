using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Sprite[] backgroundSprites;
    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetNextBackground()
    {
        if (backgroundSprites.Length == 0) return;

        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
        spriteRenderer.sprite = backgroundSprites[currentIndex];
    }

    public void SetInitialBackground()
    {
        if (backgroundSprites.Length == 0) return;

        spriteRenderer.sprite = backgroundSprites[currentIndex];
    }
}
