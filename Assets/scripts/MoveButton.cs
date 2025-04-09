using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveButton : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI moveText;
    private Button button;
    private int moveIndex;

    private GameManager gm; // Cached reference

    private void Awake()
    {
        button = GetComponent<Button>();
        gm = FindFirstObjectByType<GameManager>(); // Unity 6 replacement for deprecated FindObjectOfType
    }

    // Called by BattleUIManager to initialize this button
    public void Setup(string moveName, int index)
    {
        moveIndex = index;
        moveText.text = moveName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        if (gm != null)
        {
            gm.OnPlayerMoveSelected(moveIndex);
        }
        else
        {
            Debug.LogWarning("GameManager not found in scene!");
        }
    }
}
