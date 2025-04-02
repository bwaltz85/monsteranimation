using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveButton : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI moveText;
    private Button button;
    private int moveIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
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
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.OnPlayerMoveSelected(moveIndex);
        }
    }
}
