using UnityEngine;
using TMPro;

[ExecuteAlways]
public class LockUITransform : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 lockedAnchoredPosition;
    private Vector3 lockedScale;
    private float lockedFontSize;
    private TextMeshProUGUI tmp;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tmp = GetComponent<TextMeshProUGUI>();

        if (rectTransform != null)
        {
            lockedAnchoredPosition = rectTransform.anchoredPosition;
            lockedScale = rectTransform.localScale;
        }

        if (tmp != null)
        {
            lockedFontSize = tmp.fontSize;
        }
    }

    void LateUpdate()
    {
        // Lock position and scale
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = lockedAnchoredPosition;
            rectTransform.localScale = lockedScale;
        }

        // Lock font size
        if (tmp != null)
        {
            tmp.fontSize = lockedFontSize;
        }
    }
}
